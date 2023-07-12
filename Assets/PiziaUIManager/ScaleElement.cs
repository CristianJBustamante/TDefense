using UnityEngine;
using UnityEngine.Events;
using com.Pizia.Tools;
using com.Pizia.uiManager;

public class ScaleElement : CachedReferences, IuiElement
{
    [Header("Scale Settings")]
    [Space(5)]
    [SerializeField] Vector3 startingValue = Vector3.zero;
    [SerializeField] Vector3 finishingValue = Vector3.one;
    [SerializeField] float scaleTime = 0.2f;
    [SerializeField] LeanTweenType scaleEasing;

    [Header("PingPong Settings")]
    [Space(5)]
    [SerializeField] bool usePingPong;
    [SerializeField] Vector3 minValue = Vector3.one * 0.8f;
    [SerializeField] float pingPongTime = 0.6f;
    [SerializeField] LeanTweenType pingPongEasing;

    [Space(15)]
    [Header("Scale Events")]
    [Space(5)]
    [SerializeField] UnityEvent onShownComplete;
    [SerializeField] UnityEvent onHideComplete;

    [ContextMenu("Hide")]
    public void HideElement()
    {
        LeanTween.cancel(MyGameObject);
        LeanTween.scale(MyRectTransform, startingValue, scaleTime).setEase(scaleEasing).setOnComplete(() => onHideComplete?.Invoke());
    }

    public void SetElement()
    {
        LeanTween.cancel(MyGameObject);
        MyRectTransform.localScale = startingValue;
    }

    public void ShowElement()
    {
        LeanTween.cancel(MyGameObject);

        if(usePingPong)
        {
            LeanTween.scale(MyRectTransform, finishingValue, scaleTime).setEase(scaleEasing).setOnComplete(() =>
            {
                onShownComplete?.Invoke();
                LeanTween.scale(MyRectTransform, minValue, pingPongTime).setEase(pingPongEasing).setLoopPingPong();
            });

        }
        else
        {
            LeanTween.scale(MyRectTransform, finishingValue, scaleTime).setEase(scaleEasing).setOnComplete(() => onShownComplete?.Invoke());
        }
    }
}