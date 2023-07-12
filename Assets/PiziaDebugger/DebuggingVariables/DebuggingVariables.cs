using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

namespace Pizia
{
    public class DebuggingVariables : MonoBehaviour
    {
        public Transform spawnPivot;
        public GameObject debuggingVariableObj;
        public List<GameObject> instancedObjs;

        public void LoadData()
        {
            if (instancedObjs != null) 
            {
                for (int i=0; i<instancedObjs.Count; i++) 
                {
                    Destroy(instancedObjs[i]);
                }
            }
            instancedObjs = new List<GameObject>();

            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mono in sceneActive) 
            {
                FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < objectFields.Length; i++) 
                {
                    DebugField attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(DebugField)) as DebugField;
                    if (attribute != null)
                    {
                        GameObject _obj = Instantiate(debuggingVariableObj,spawnPivot);
                        _obj.GetComponent<DebuggingVariable>().Initialize(objectFields[i],mono);
                        instancedObjs.Add(_obj);
                    }
                }
            }

            GetComponent<LayoutGroup>().SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }
}