using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BooleanUI : MonoBehaviour
{
    public RectTransform dotPosition;
    public Image dotColor;
    public UnityEvent onSwitch;


    public bool boolState;


    public void Init(bool _bool) {
        boolState = _bool;
        if (_bool) {
            dotColor.color = new Color(1,0.75f,0.35f,1);
            dotPosition.anchoredPosition = new Vector3(25,0,0);
        } else {
            dotColor.color = new Color(0.5f,0.5f,0.5f,1);
            dotPosition.anchoredPosition = new Vector3(-25,0,0);
        }
    }

    public void SetBoolean(bool _bool) {
        boolState = _bool;
        if (_bool) {
            dotColor.color = new Color(1,0.75f,0.35f,1);
            LeanTween.move(dotPosition,new Vector3(25,0,0),0.1f);
        } else {
            dotColor.color = new Color(0.5f,0.5f,0.5f,1);
            LeanTween.move(dotPosition,new Vector3(-25,0,0),0.1f);
        }
    }

    public void SwitchBool () {
        SetBoolean(!boolState);
        onSwitch.Invoke();
    }
}
