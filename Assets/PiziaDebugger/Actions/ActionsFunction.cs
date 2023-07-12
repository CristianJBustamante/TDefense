using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

public class ActionsFunction : MonoBehaviour
{
    private MethodInfo methodInfo;
    private MonoBehaviour parentObj;
    public Text text;
    public Color darkColor;

    public void Initialize (MethodInfo _methodInfo, MonoBehaviour _parentObj, bool isDark) {
        methodInfo = _methodInfo;
        parentObj = _parentObj;
        text.text = methodInfo.Name;
        
        if (isDark) {
            GetComponent<Image>().color = darkColor;
        }
    }

    public void ExecuteCommand () {
        methodInfo.Invoke(parentObj,new System.Object[]{}); 
    }
}
