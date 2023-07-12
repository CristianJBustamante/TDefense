using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class ActionsVariable : MonoBehaviour
{
    FieldInfo fieldInfo;
    MonoBehaviour parentObj;
    public Text text;
    public InputField inputField;
    public BooleanUI booleanUI;
    public Vector3UI vector3UI;
    public Vector2UI vector2UI;
    public Color darkColor;

    private int type = -1; //0 int, 1 float, 2 string, 3 bool

    public void Initialize (FieldInfo _fieldInfo, MonoBehaviour _parentObj, bool isDark) {
        fieldInfo = _fieldInfo;
        parentObj = _parentObj;
        text.text = fieldInfo.Name;

        if (isDark) {
            GetComponent<Image>().color = darkColor;
        }

        //Debug.Log(fieldInfo.FieldType);

        if (fieldInfo.FieldType.Equals(typeof(int))){
            inputField.text = fieldInfo.GetValue(parentObj).ToString();
            inputField.contentType = InputField.ContentType.IntegerNumber;
            type = 0;
        } else if (fieldInfo.FieldType.Equals(typeof(float))){
            inputField.text = fieldInfo.GetValue(parentObj).ToString();
            inputField.contentType = InputField.ContentType.DecimalNumber;
            type = 1;
        } else if (fieldInfo.FieldType.Equals(typeof(string))){
            inputField.text = fieldInfo.GetValue(parentObj).ToString();
            inputField.contentType = InputField.ContentType.Standard;
            type = 2;
        } else if (fieldInfo.FieldType.Equals(typeof(bool))){
            inputField.gameObject.SetActive(false);
            booleanUI.gameObject.SetActive(true);
            booleanUI.Init((bool)fieldInfo.GetValue(parentObj));
            type = 3;
        } else if (fieldInfo.FieldType.Equals(typeof(Vector3))){
            inputField.gameObject.SetActive(false);
            vector3UI.gameObject.SetActive(true);
            vector3UI.Init((Vector3)fieldInfo.GetValue(parentObj));
            type = 4;
        } else if (fieldInfo.FieldType.Equals(typeof(Vector2))){
            inputField.gameObject.SetActive(false);
            vector2UI.gameObject.SetActive(true);
            vector2UI.Init((Vector2)fieldInfo.GetValue(parentObj));
            type = 5;
        }

    }

    public void OnTextChanged () {
        if (type == 0) {
            fieldInfo.SetValue(parentObj,int.Parse(inputField.text));
        } else if (type == 1) {
            fieldInfo.SetValue(parentObj,float.Parse(inputField.text));
        } else if (type == 2) {
            fieldInfo.SetValue(parentObj,inputField.text);
        }
    }

    public void OnBooleanChanged (BooleanUI _booleanUI) {
        fieldInfo.SetValue(parentObj,_booleanUI.boolState); 
    }

    public void OnVector3Changed (Vector3UI _vector3UI){
        fieldInfo.SetValue(parentObj,_vector3UI.value);
    }

    public void OnVector2Changed (Vector2UI _vector2UI){
        fieldInfo.SetValue(parentObj,_vector2UI.value);
    }
}
