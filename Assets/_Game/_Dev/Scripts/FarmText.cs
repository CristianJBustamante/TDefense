using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;
using TMPro;

public class FarmText : CachedReferences
{
    [Header("Properties")]
    public bool animating;
    public float animTime;
    public float onScreenTime;

    [Header("Components")]
    public GameObject pivot;
    public SpriteRenderer sprite;
    public TMP_Text text;

    Transform originalParent;
    [HideInInspector] public Transform currentParent;
    Vector3 startPos;

    int moveTweenID;
    int scaleTweenID;

    //====================================================================================

    private void Awake()
    {
        originalParent = transform.parent;
        startPos = transform.position;
        Reset();
    }

    //====================================================================================

    public void Animate(int _amount, Sprite _icon = null, float _y = 0, Transform _parent = null)
    {
        StartCoroutine(AnimateCoroutine("+" + _amount.ToString(), _icon, _y, _parent));
    }
    public void Animate(string _text, Sprite _icon = null, float _y = 0, Transform _parent = null)
    {
        StartCoroutine(AnimateCoroutine(_text, _icon, _y, _parent));
    }

    IEnumerator AnimateCoroutine(string _text, Sprite _icon = null, float _y = 0, Transform _parent = null)
    {
        LeanTween.cancel(moveTweenID);
        LeanTween.cancel(scaleTweenID);

        text.text = _text;
        if (_icon != null) sprite.sprite = _icon;
        if (_parent != null)
        {
            if (_parent != currentParent) currentParent = _parent;
            transform.parent = currentParent;
            transform.localPosition *= 0;
        }
        pivot.transform.localPosition = new Vector3(0, 2f + _y, 0f);
        if (!_text.Contains("+")) pivot.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y, pivot.transform.position.z - 2f);

        moveTweenID = LeanTween.moveLocalY(pivot, pivot.transform.localPosition.y + (2.5f * currentParent.transform.localScale.y), animTime).setEaseOutExpo().uniqueId;
        scaleTweenID = LeanTween.scale(pivot, Vector3.one, animTime * 0.5f).setEaseOutExpo().uniqueId;

        animating = true;

        yield return new WaitForSeconds(animTime + onScreenTime);

        float outTime = animTime * 0.35f;
        scaleTweenID = LeanTween.scale(pivot, Vector3.zero, outTime).uniqueId;

        yield return new WaitForSeconds(outTime);

        Reset();
    }

    public void Reset()
    {
        transform.parent = originalParent;
        transform.position = startPos;
        pivot.transform.localPosition = new Vector3(0f, 2f, 0f);
        pivot.transform.localScale *= 0;
        animating = false;
    }
}
