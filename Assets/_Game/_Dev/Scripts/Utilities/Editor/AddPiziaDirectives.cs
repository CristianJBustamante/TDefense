using System.IO;
using UnityEditor;
using System.Text;

public class AddPiziaDirectives : UnityEditor.AssetModificationProcessor
{
    static StringBuilder Word = new StringBuilder();
    static StringBuilder NewContent = new StringBuilder();
    static StringBuilder Aux = new StringBuilder();

    public static void OnWillCreateAsset(string metaFilePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(metaFilePath);

        if (!fileName.EndsWith(".cs")) return;

        string actualFilePath = Path.Combine(Path.GetDirectoryName(metaFilePath), fileName);

        string content = File.ReadAllText(actualFilePath);
        string newContent = content;
        
        AddDirectives(ref newContent, fileName.StartsWith("UI"), fileName.EndsWith("Data.cs"), Path.GetFileNameWithoutExtension(fileName));

        if (content != newContent)
        {
            File.WriteAllText(actualFilePath, newContent);
            AssetDatabase.Refresh();
        }
    }

    static void AddDirectives(ref string content, bool addUIDirective = false, bool addScriptableDirective = false, string fileName = "")
    {
        if (content.Contains("using com.Pizia.Tools") || content.Contains("using com.Pizia.Saver") || (addUIDirective && content.Contains("using UnityEngine.UI"))) 
            return;

        int lenght = content.Length;

        Word.Clear();
        NewContent.Clear();
        Aux.Clear();

        for (int i = 0; i < lenght; i++)
        {
            if (Word.ToString() == "using")
            {
                if (content[i] == '\n')
                    Word.Clear();
            }
            else
            {
                Word.Append(content[i]);
            }

            if (Word.ToString() == "\r\n")
            {
                NewContent.Remove(NewContent.Length - 1, 1);
                if (addUIDirective) NewContent.AppendFormat("using UnityEngine.UI;{0}using TMPro;{0}", System.Environment.NewLine);
                NewContent.AppendFormat("using com.Pizia.Tools;{0}using com.Pizia.Saver;{0}{0}", System.Environment.NewLine);
                if (addScriptableDirective) NewContent.AppendFormat("[CreateAssetMenu(fileName = \"{0}\", menuName = \"Data/{0}\")]{1}", fileName, System.Environment.NewLine);

                for (int j = i + 1; j < lenght; j++) NewContent.Append(content[j]);

                if (addScriptableDirective)
                {
                    NewContent.Replace("MonoBehaviour", "ScriptableObject");
                    Aux.AppendFormat("    // Start is called before the first frame update{0}", System.Environment.NewLine);
                    NewContent.Replace(Aux.ToString(), "");
                    Aux.Clear();


                    Aux.AppendFormat("    void Start(){0}    {{{0}        {0}    }}{0}{0}", System.Environment.NewLine);
                    NewContent.Replace(Aux.ToString(), "");
                    Aux.Clear();

                    Aux.AppendFormat("    // Update is called once per frame{0}", System.Environment.NewLine);
                    NewContent.Replace(Aux.ToString(), "");
                    Aux.Clear();

                    Aux.AppendFormat("    void Update(){0}    {{{0}        {0}    }}{0}", System.Environment.NewLine);
                    NewContent.Replace(Aux.ToString(), System.Environment.NewLine);
                    Aux.Clear();

                }
                else
                {
                    NewContent.Replace("MonoBehaviour", "CachedReferences");
                    Aux.AppendFormat("    // Start is called before the first frame update{0}", System.Environment.NewLine);
                    NewContent.Replace(Aux.ToString(), "");
                    Aux.Clear();

                    Aux.AppendFormat("    // Update is called once per frame{0}", System.Environment.NewLine);
                    NewContent.Replace(Aux.ToString(), "");
                    Aux.Clear();
                }

                content = NewContent.ToString();
                break;
            }

            NewContent.Append(content[i]);
        }
    }
}
