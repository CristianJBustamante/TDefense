using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Vector3UI : MonoBehaviour
{
    public InputField x;
    public InputField y;
    public InputField z;

    public Vector3 value;
    public UnityEvent onSomethingChanged;

    public void Init(Vector3 vector){
        value = vector;
        x.text = vector.x.ToString();
        y.text = vector.y.ToString();
        z.text = vector.z.ToString();
    }

    public void OnChangedX(){
        value.x = float.Parse(x.text);
        onSomethingChanged.Invoke();
    }

    public void OnChangedY(){
        value.y = float.Parse(y.text);
        onSomethingChanged.Invoke();
    }

    public void OnChangedZ(){
        value.z = float.Parse(z.text);
        onSomethingChanged.Invoke();
    }
}
