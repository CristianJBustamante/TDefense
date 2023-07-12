using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Vector2UI : MonoBehaviour
{
    public InputField x;
    public InputField y;

    public Vector2 value;
    public UnityEvent onSomethingChanged;

    public void Init(Vector2 vector){
        value = vector;
        x.text = vector.x.ToString();
        y.text = vector.y.ToString();
    }

    public void OnChangedX(){
        value.x = float.Parse(x.text);
        onSomethingChanged.Invoke();
    }

    public void OnChangedY(){
        value.y = float.Parse(y.text);
        onSomethingChanged.Invoke();
    }
}
