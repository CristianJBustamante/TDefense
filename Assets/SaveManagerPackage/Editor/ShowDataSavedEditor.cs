using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using com.Pizia.Tools;
using com.Pizia.Saver;

namespace com.Pizia.Saver
{
    [CustomEditor(typeof(ShowDataSaved))]
    public class ShowDataSavedEditor : Editor
    {
        // % - Ctrl  # - Shift  & - alt  _ - no key modifier
        const string SHORTCUT = "#u";

        SerializedProperty enableInsantSaveProperty, timeSavingProperty, saveOnSceneChangeProperty, timeToSaveProperty;

        bool boolShow, intShow, longShow, floatShow, stringShow, vector3Show, arrayShow, dictionaryShow, saveChanges, autoSave;
        bool[] arrayDataShow, dictionaryDataShow;

        DataSave SaveData => SaveManager.SaveData;

        void OnEnable()
        {
            enableInsantSaveProperty = serializedObject.FindProperty("enableInsantSave");
            timeSavingProperty = serializedObject.FindProperty("timeSaving");
            saveOnSceneChangeProperty = serializedObject.FindProperty("saveOnSceneChange");
            timeToSaveProperty = serializedObject.FindProperty("timeToSave");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //EditorGUI.BeginDisabledGroup(true);

            ShowDataArrays();

            //EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            enableInsantSaveProperty.boolValue = EditorGUILayout.Toggle("Enable Instant Save", enableInsantSaveProperty.boolValue);
            saveOnSceneChangeProperty.boolValue = EditorGUILayout.Toggle("Enable Save on Scene change", saveOnSceneChangeProperty.boolValue);

            timeSavingProperty.boolValue = EditorGUILayout.Toggle("Enable Saving by Time", timeSavingProperty.boolValue);
            if (timeSavingProperty.boolValue) timeToSaveProperty.floatValue = EditorGUILayout.FloatField("Time to save", timeToSaveProperty.floatValue);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            autoSave = PlayerPrefs.GetInt("SaveManager_AutoSave") == 1;
            autoSave = EditorGUILayout.Toggle("Enable Auto Save", autoSave);
            if (!autoSave && GUILayout.Button("Save Changes")) saveChanges = true;
            
            if (saveChanges)
            {
                SaveManager.SaveAll();
                saveChanges = false;
            }
            
            if (GUILayout.Button("Clear Save")) Clear();
            serializedObject.ApplyModifiedProperties();
            PlayerPrefs.SetInt("SaveManager_AutoSave", autoSave ? 1 : 0);
        }

        void ShowDataArrays()
        {
            List<string> keyList = new List<string>();

            boolShow = EditorGUILayout.Foldout(boolShow, "Saved Bools");
            if (boolShow) 
            {
                EditorGUI.indentLevel = 1;
                GetKeys(ref keyList, SaveData.boolSave.Keys);
                int size = keyList.Count;
                
                for (int i = 0; i < size; i++)
                {
                    bool temp = SaveData.boolSave[keyList[i]];
                    SaveData.boolSave[keyList[i]] = EditorGUILayout.Toggle(keyList[i], SaveData.boolSave[keyList[i]]);
                    if (autoSave && temp != SaveData.boolSave[keyList[i]]) saveChanges = true;
                }
            }
            EditorGUI.indentLevel = 0;

            intShow = EditorGUILayout.Foldout(intShow, "Saved Ints");
            if (intShow)
            {
                EditorGUI.indentLevel = 1;
                GetKeys(ref keyList, SaveData.intSave.Keys);
                int size = keyList.Count;

                for (int i = 0; i < size; i++)
                {
                    int temp = SaveData.intSave[keyList[i]];
                    SaveData.intSave[keyList[i]] = EditorGUILayout.IntField(keyList[i], SaveData.intSave[keyList[i]]);
                    if (autoSave && temp != SaveData.intSave[keyList[i]]) saveChanges = true;
                }
            }
            EditorGUI.indentLevel = 0;

            longShow = EditorGUILayout.Foldout(longShow, "Saved Longs");
            if (longShow)
            {
                EditorGUI.indentLevel = 1;
                GetKeys(ref keyList, SaveData.longSave.Keys);
                int size = keyList.Count;

                for (int i = 0; i < size; i++)
                {
                    long temp = SaveData.longSave[keyList[i]];
                    SaveData.longSave[keyList[i]] = EditorGUILayout.LongField(keyList[i], SaveData.longSave[keyList[i]]);
                    if (autoSave && temp != SaveData.longSave[keyList[i]]) saveChanges = true;
                }
            }
            EditorGUI.indentLevel = 0;

            floatShow = EditorGUILayout.Foldout(floatShow, "Saved Floats");
            if (floatShow) 
            { 
                EditorGUI.indentLevel = 1;
                GetKeys(ref keyList, SaveData.floatSave.Keys);
                int size = keyList.Count;

                for (int i = 0; i < size; i++)
                {
                    float temp = SaveData.floatSave[keyList[i]];
                    SaveData.floatSave[keyList[i]] = EditorGUILayout.FloatField(keyList[i], SaveData.floatSave[keyList[i]]);
                    if (autoSave && temp != SaveData.floatSave[keyList[i]]) saveChanges = true;
                }
            }
            EditorGUI.indentLevel = 0;

            stringShow = EditorGUILayout.Foldout(stringShow, "Saved Strings");
            if (stringShow) 
            {
                EditorGUI.indentLevel = 1;
                GetKeys(ref keyList, SaveData.stringSave.Keys);
                int size = keyList.Count;

                for (int i = 0; i < size; i++)
                {
                    string temp = SaveData.stringSave[keyList[i]];
                    SaveData.stringSave[keyList[i]] = EditorGUILayout.TextField(keyList[i], SaveData.stringSave[keyList[i]]);
                    if (autoSave && temp != SaveData.stringSave[keyList[i]]) saveChanges = true;
                }
            }
            EditorGUI.indentLevel = 0;

            vector3Show = EditorGUILayout.Foldout(vector3Show, "Saved Vector3");
            if (vector3Show)
            { 
                EditorGUI.indentLevel = 1;
                GetKeys(ref keyList, SaveData.vector3Save.Keys);
                int size = keyList.Count;

                for (int i = 0; i < size; i++)
                {
                    Vector3 temp = SaveData.vector3Save[keyList[i]];
                    SaveData.vector3Save[keyList[i]] = EditorGUILayout.Vector3Field(keyList[i], SaveData.vector3Save[keyList[i]]);
                    if (autoSave && temp != SaveData.vector3Save[keyList[i]]) saveChanges = true;
                }
            }
            EditorGUI.indentLevel = 0;

            SwitchDataArray();
            EditorGUI.indentLevel = 0;

            SwitchDataDictionary();
            EditorGUI.indentLevel = 0;
        }

        void SwitchDataArray()
        {
            arrayShow = EditorGUILayout.Foldout(arrayShow, "Saved Arrays");
            
            if (arrayShow)
            {
                EditorGUI.indentLevel = 1;
                CheckDataArray();

                int index = 0;
                foreach (string item in SaveData.arraySave.Keys)
                {
                    arrayDataShow[index] = EditorGUILayout.Foldout(arrayDataShow[index], item);
                    if (arrayDataShow[index++]) SwitchArray(SaveData.arraySave[item].array, SaveData.arraySave[item].type);
                }
            }
        }

        void CheckDataArray()
        {
            if (arrayDataShow == null) arrayDataShow = new bool[SaveData.arraySave.Count];
            if (arrayDataShow.Length != SaveData.arraySave.Count)
            {
                bool[] temp = arrayDataShow;
                arrayDataShow = new bool[SaveData.arraySave.Count];

                int size = temp.Length < SaveData.arraySave.Count ? temp.Length : SaveData.arraySave.Count;
                for (int i = 0; i < size; i++) arrayDataShow[i] = temp[i];
            }
        }

        void SwitchArray(IList list, System.Type type)
        {
            EditorGUI.indentLevel = 2;
            
            int size = list.Count, index = -1;
            object temp = null;
            for (int i = 0; i < size; i++)
            {
                temp = list[i];
                index = i;
                if (type == typeof(bool)) { list[i] = EditorGUILayout.Toggle((bool) list[i]); continue; }
                if (type == typeof(byte) || type == typeof(short) || type == typeof(int)) { list[i] = EditorGUILayout.IntField((int) list[i]); continue; }
                if (type == typeof(long)) { list[i] = EditorGUILayout.LongField((long) list[i]); continue; }
                if (type == typeof(float)) { list[i] = EditorGUILayout.FloatField((float) list[i]); continue; }
                if (type == typeof(double)) { list[i] = EditorGUILayout.DoubleField((double) list[i]); continue; }
                if (type == typeof(char) || type == typeof(string)) { list[i] = EditorGUILayout.TextField((string) list[i]); continue; }
                if (type == typeof(Vector3)) { list[i] = EditorGUILayout.Vector3Field("Vector", (Vector3) list[i]); continue; }
            }

            if (autoSave && temp != null && temp != list[index]) saveChanges = true;
            EditorGUI.indentLevel = 1;
        }

        void SwitchDataDictionary()
        {
            dictionaryShow = EditorGUILayout.Foldout(dictionaryShow, "Saved Dictionaries");

            if (dictionaryShow)
            {
                EditorGUI.indentLevel = 1;
                CheckDataDictionary();

                int index = 0;
                foreach (string item in SaveData.dictionarySave.Keys)
                {
                    dictionaryDataShow[index] = EditorGUILayout.Foldout(dictionaryDataShow[index], item);
                    if (dictionaryDataShow[index++]) SwitchArray(SaveData.dictionarySave[item].values, SaveData.dictionarySave[item].dictionaryType);
                }
            }
        }

        void CheckDataDictionary()
        {
            if (dictionaryDataShow == null) dictionaryDataShow = new bool[SaveData.dictionarySave.Count];
            if (dictionaryDataShow.Length != SaveData.dictionarySave.Count)
            {
                bool[] temp = dictionaryDataShow;
                dictionaryDataShow = new bool[SaveData.dictionarySave.Count];

                int size = temp.Length < SaveData.dictionarySave.Count ? temp.Length : SaveData.dictionarySave.Count;
                for (int i = 0; i < size; i++) dictionaryDataShow[i] = temp[i];
            }
        }
        
        void GetKeys<T>(ref List<string> keysList, Dictionary<string, T>.KeyCollection keys)
        {
            keysList.Clear();
            foreach (string key in keys) keysList.Add(key);
        }

        [MenuItem("Pizia/Clear Save File " + SHORTCUT)]
        static void Clear()
        {
            Debug.Log("SaveManager: Cleared file");
            Saver.Clear();
        }
    }
}
