using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pizia
{
    public class LogElement : MonoBehaviour
    {
        public Text text;
        public Image image;
        public Image background;
        LayoutGroup layoutGroup;

        public Sprite logSrite;
        public Sprite warningSprite;
        public Sprite errorSprite;

        public LogType logType;
        private Console console;
        private string trace;

        public void InitializeLog (string log,bool applyBackground, string stacktrace = "", LogType type = LogType.Log) 
        {
            if (stacktrace == null) {
                trace = "";
            }

            trace = stacktrace;
            logType = type;
            text.text = log;

            if (applyBackground) 
            {
                background.color = new Color(0.13f,0.13f,0.13f);
            }

            if (type == LogType.Log) 
            {
                image.sprite = logSrite;
            } 
            else if (type == LogType.Warning) 
            {
                image.sprite = warningSprite;
            } 
            else 
            {
                image.sprite = errorSprite;
            }
            console = GetComponentInParent<Console>();
        }

        public void ShowTrace () 
        {
            if (console == null) {
                console = GetComponentInParent<Console>();
            }

            console.ShowTrace(trace);
        }
    }
}