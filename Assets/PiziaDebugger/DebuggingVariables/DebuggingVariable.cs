using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace Pizia
{
    public class DebuggingVariable : MonoBehaviour
    {
        public FieldInfo fieldInfo;
        public MonoBehaviour parentObj;
        public Text text;
        private int type = -1;

        private int intValue;
        private float floatValue;
        private string stringValue;
        private bool boolValue;

        public void Initialize (FieldInfo _fieldInfo, MonoBehaviour _parentObj) 
        {
            fieldInfo = _fieldInfo;
            parentObj = _parentObj;
            text.text = fieldInfo.Name + ": ";
            
            if (fieldInfo.FieldType.Equals(typeof(int)))
            {
                intValue = (int)fieldInfo.GetValue(parentObj);
                text.text += intValue.ToString();
                type = 0;
            } 
            else if (fieldInfo.FieldType.Equals(typeof(float)))
            {
                floatValue = (float)fieldInfo.GetValue(parentObj);
                text.text += floatValue.ToString();
                type = 1;
            } 
            else if (fieldInfo.FieldType.Equals(typeof(string)))
            {
                stringValue = (string)fieldInfo.GetValue(parentObj);
                text.text += stringValue;
                type = 2;
            } 
            else if (fieldInfo.FieldType.Equals(typeof(bool)))
            {
                boolValue = (bool)fieldInfo.GetValue(parentObj);
                text.text += boolValue.ToString();
                type = 3;
            }
        }

        private void Update ()
        {
            switch (type) 
            {
                case 0:
                    if ((int)fieldInfo.GetValue(parentObj) != intValue) 
                    {
                        intValue = (int)fieldInfo.GetValue(parentObj);
                        text.text = fieldInfo.Name + ": " + intValue.ToString();
                    }
                break;
                case 1:
                    if ((float)fieldInfo.GetValue(parentObj) != floatValue) 
                    {
                        floatValue = (float)fieldInfo.GetValue(parentObj);
                        text.text = fieldInfo.Name + ": " + floatValue.ToString();
                    }
                break;
                case 2:
                    if ((string)fieldInfo.GetValue(parentObj) != stringValue) 
                    {
                        stringValue = (string)fieldInfo.GetValue(parentObj);
                        text.text = fieldInfo.Name + ": " + stringValue;
                    }
                break;
                case 3:
                    if ((bool)fieldInfo.GetValue(parentObj) != boolValue) 
                    {
                        boolValue = (bool)fieldInfo.GetValue(parentObj);
                        text.text = fieldInfo.Name + ": " + boolValue.ToString();
                    }
                break;
            }
        }
    }
}