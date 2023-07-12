using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Reflection;

namespace Pizia
{
    public class Command 
    {
        public string trigger;
        public int minParameters;
        public int maxParameters;
        private Action<string[]> action;

        public Command (string _trigger,int _minParameters, int _maxParameters, Action<string[]> _action) 
        {
            trigger = _trigger;
            minParameters = _minParameters;
            maxParameters = _maxParameters;
            action = _action;
        }

        public void ExecuteCommand (string[] parameters) 
        {
            bool errorFound = false; 
            if (parameters.Length < minParameters || parameters.Length > maxParameters) 
            {
                errorFound = true;
                if (minParameters == maxParameters) 
                {
                    string aux = "";
                    if (minParameters > 1) aux = "s";
                    Debug.LogError($"<color={Console.instance.errorColor}>ERROR: {trigger} requires exactly {minParameters} parameter{aux}</color>");
                } 
                else 
                {
                    Debug.LogError($"<color={Console.instance.errorColor}>ERROR: {trigger} requires between {minParameters} and {maxParameters} parameters</color>");
                }
            } 
            if (!errorFound) 
            {
                action(parameters); 
            }
        }
    }

    public class Console : MonoBehaviour
    {
        public static Console instance;
        public string highlightColor;
        public string errorColor;
        public Transform viewport;
        public GameObject logElementObj;
        public TraceLog traceLog;
        public List<Command> commands;
        public ErrorNotification errorNotification;
        [Space(10)]
        public bool showingLogs = true;
        public bool showingWarnings = true;
        public bool showingErrors = true;

        private bool showLogBackground = false;
        private List<LogElement> elements;

        public void Initialize () 
        {
            instance = this;
            commands = new List<Command>();
            commands.Add(new Command("ls",0,0,Command_LS));
            commands.Add(new Command("sum",2,2,Command_SUM));
            commands.Add(new Command("ggv",1,1,Command_GetGlobalVariable));

            elements = new List<LogElement>();
            Application.logMessageReceivedThreaded += LogMessageReceived;
        }

        public static void AddCommand (Command command) 
        {
            Console.instance.commands.Add(command);
        }

        public void ShowTrace (string trace) 
        {
            traceLog.ShowTraceLog(trace);
        }

        private void LogMessageReceived (string logString, string stackTrace, LogType type) 
        {
            LogElement log = Instantiate(logElementObj,viewport).GetComponent<LogElement>();

            string[] linesInFile = stackTrace.Split('\n');
            for(int i=0; i<linesInFile.Length; i++) {
                if (linesInFile[i].Contains(".cs:")) {
                    string[] aux = linesInFile[i].Split('(');;
                    for (int n=0; n<aux.Length; n++) {
                        if (aux[n].Contains("at ")){
                            string finalString = aux[n].Remove(0, 3);
                            finalString  = finalString.Substring(0,finalString.Length-1);
                            logString += "<size=18>\n(" + finalString + ")</size>";
                            break;
                        }
                    }
                    break;
                }
            }

            log.InitializeLog(logString,showLogBackground,stackTrace,type);
            showLogBackground = !showLogBackground;

            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) {
                errorNotification.ShowError(logString);
                if (!showingErrors) 
                {
                    log.gameObject.SetActive(false);
                }
            } 
            else if (type == LogType.Warning && !showingWarnings)
            {
                log.gameObject.SetActive(false);
            }
            else if (type == LogType.Log && !showingLogs)
            {
                log.gameObject.SetActive(false);
            }

            elements.Add(log);

        }

        private void RebuildLayout () 
        {
            LayoutGroup[] childLayoutGroups = gameObject.GetComponentsInChildren<LayoutGroup>();
            for (int i=0; i<childLayoutGroups.Length; i++) 
            {
                childLayoutGroups[i].CalculateLayoutInputVertical();
                childLayoutGroups[i].CalculateLayoutInputHorizontal();
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)childLayoutGroups[i].transform);
                LayoutRebuilder.MarkLayoutForRebuild((RectTransform)childLayoutGroups[i].transform);
            }
        }

        private void Command_LS (string[] parameters) 
        {
            for (int i=0; i<commands.Count; i++) 
            {
                Debug.Log($"<color={highlightColor}>{commands[i].trigger.ToUpper()}</color> > parameters {commands[i].minParameters}/{commands[i].maxParameters}");
            }
        }

        private void Command_SUM (string[] parameters) 
        {
            int a = int.Parse(parameters[0]);
            int b = int.Parse(parameters[1]);
            int result = a + b;

            Debug.Log($"<color={highlightColor}>SUM:</color> {a}+{b}={result.ToString()}");
        }

        private void Command_GetGlobalVariable (string[] parameters) 
        {
            var _class = Type.GetType(parameters[0]);
            
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;
            FieldInfo[] fields = _class.GetFields(bindingFlags);

            for (int i=0; i<fields.Length; i++) 
            {
                Debug.Log($"{fields[i].Name}: {fields[i].GetValue(_class)}");
            }
        }

        public void SendCommand (InputField inputField) 
        {
            string[] text = inputField.text.Split(' ');
            inputField.text = "";
            //inputField.ActivateInputField();


            if (text.Length > 0 && text[0] != string.Empty) 
            {
                string command = text[0].ToLower();
                string[] parameters = new string[text.Length-1];

                for (int i=0; i<parameters.Length; i++) 
                {
                    parameters[i] = text[i+1];
                }

                bool commandFound = false;
                for (int i=0; i<commands.Count; i++) 
                {
                    if (commands[i].trigger == command) 
                    {
                        commands[i].ExecuteCommand(parameters);
                        commandFound = true;
                        break;
                    }
                }

                if (!commandFound) 
                {
                    Debug.LogError($"<color={errorColor}>ERROR: Command {command} not found!</color>");
                }
            }
        }

        public void SwitchElementsVisibility (int typeIndex, bool show) 
        {
            for (int i=0; i<elements.Count; i++) 
            {
                if (elements[i].logType == LogType.Log && typeIndex == 0) 
                {
                    elements[i].gameObject.SetActive(show);
                }
                else if (elements[i].logType == LogType.Warning && typeIndex == 1)
                {
                    elements[i].gameObject.SetActive(show);
                }
                else if (elements[i].logType == LogType.Error && typeIndex == 2)
                {
                    elements[i].gameObject.SetActive(show);
                }
                else if (elements[i].logType == LogType.Assert && typeIndex == 2)
                {
                    elements[i].gameObject.SetActive(show);
                }
                else if (elements[i].logType == LogType.Exception && typeIndex == 2)
                {
                    elements[i].gameObject.SetActive(show);
                }
            }
        }

        public void CleanAllLogs ()
        {
            for (int i=0; i<elements.Count; i++)
            {
                Destroy(elements[i].gameObject);
            }
            elements = new List<LogElement>();
        }
    }
}
