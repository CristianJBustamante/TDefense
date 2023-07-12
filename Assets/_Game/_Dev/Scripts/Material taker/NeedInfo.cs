using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NeedInfo : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] TMP_Text infoText;
    [SerializeField] SpriteRenderer rend;

    //================================================================================

    private void Awake()
    {
        // transform.localScale = Vector3.zero;
    }

    //================================================================================

    public void SetText(string _text) => infoText.text = _text;
    public void SetIcon(Sprite _spr) => rend.sprite = _spr;
    public void AnimateIn(float _delay, float _yPos)
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.2f).setEaseOutBack().setDelay(_delay);
        transform.localPosition = new Vector3(transform.localPosition.x, _yPos, transform.localPosition.z);
    }
    public void AnimateOut() => LeanTween.scale(gameObject, Vector3.zero, 0.2f).setEaseOutQuad();

}