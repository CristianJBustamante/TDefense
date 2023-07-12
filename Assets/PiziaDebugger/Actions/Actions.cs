using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Pizia
{
    public class Actions : MonoBehaviour
    {
        public GameObject fieldPrefab;
        public GameObject functionPrefab;

        [Space(20)]
        public Transform generalPivot;
        public List<GameObject> instantedPanels;

        private void OnEnable() {
            LoadData();
        }

        private void OnDisable() {
            for (int i=0; i<instantedPanels.Count; i++) {
                Destroy(instantedPanels[i]);
            }
        }

        private void LoadData() {
            instantedPanels = new List<GameObject>();
            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            bool isDark = false;

            foreach (MonoBehaviour mono in sceneActive) {
                MethodInfo[] objectMethods = mono.GetType().GetMethods();
                for (int i = 0; i < objectMethods.Length; i++) {
                    ConsoleFunction attribute = Attribute.GetCustomAttribute(objectMethods[i], typeof(ConsoleFunction)) as ConsoleFunction;
                    if (attribute != null) {
                        GameObject _obj = Instantiate(functionPrefab,generalPivot);
                        _obj.GetComponent<ActionsFunction>().Initialize(objectMethods[i],mono,isDark);
                        _obj.transform.SetSiblingIndex(1);
                        instantedPanels.Add(_obj);
                        
                        isDark = !isDark;
                    }
                }

                FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                for (int i = 0; i < objectFields.Length; i++) {
                    ConsoleField attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(ConsoleField)) as ConsoleField;
                    if (attribute != null){
                        GameObject _obj = Instantiate(fieldPrefab,generalPivot);
                        _obj.GetComponent<ActionsVariable>().Initialize(objectFields[i],mono,isDark);
                        _obj.transform.SetAsLastSibling();
                        instantedPanels.Add(_obj);

                        isDark = !isDark;
                    }
                }
            }
        }
    }
}
