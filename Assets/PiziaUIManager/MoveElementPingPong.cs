using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.uiManager;
using UnityEngine.Events;


public class MoveElementPingPong : CachedReferences, IuiElement
{
    [Header("Move Settings")]
    [Space(5)]
    [SerializeField] Transform initPos;
    [SerializeField] Transform finalPos;
    [SerializeField] float moveTime = 0.2f;
    [SerializeField] LeanTweenType moveEasing;

    [Header("Scale Settings")]
    [Space(5)]
    [SerializeField] bool useScale = true;
    [SerializeField] Vector3 startingValue = Vector3.zero;
    [SerializeField] Vector3 finishingValue = Vector3.one;
    [SerializeField] float scaleTime = 0.2f;
    [SerializeField] LeanTweenType scaleEasing;

    [Space(15)]
    [Header("Move Events")]
    [Space(5)]
    [SerializeField] UnityEvent onShownComplete;
    [SerializeField] UnityEvent onHideComplete;

    public void HideElement()
    {
        LeanTween.cancel(MyGameObject);
        LeanTween.scale(MyGameObject, startingValue, scaleTime).setEase(scaleEasing).setOnComplete(() => onHideComplete?.Invoke());
    }

    public void SetElement()
    {
        LeanTween.cancel(MyGameObject);

        if (useScale) MyRectTransform.localScale = startingValue;

        MyRectTransform.position = initPos.position;
    }

    public void ShowElement()
    {
        LeanTween.cancel(MyGameObject);
        if (useScale)
        {
            LeanTween.scale(MyGameObject, finishingValue, scaleTime).setEase(scaleEasing).setOnComplete(() =>
            {
                onShownComplete?.Invoke();
                LeanTween.move(MyGameObject, finalPos.position, moveTime).setEase(moveEasing).setLoopPingPong();
            });
        }
        else
        {
            LeanTween.move(MyGameObject, finalPos.position, moveTime).setEase(moveEasing).setLoopPingPong();
        }
    }
}
