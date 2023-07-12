using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum InteractionType { MaterialTaker, NPC }
public class InteractionArea : MonoBehaviour
{
    [SerializeField] Collider interiorCol;

    [Header("Components")]
    [SerializeField] GameObject area;
    [SerializeField] Image interactionLoadImg;
    [SerializeField] CanvasGroup loadImgAlpha;
    [SerializeField] Renderer areaRend;

    public delegate void OnEnter();
    public OnEnter onEnter;
    public delegate void OnExit();
    public OnExit onExit;

    int readyTweenID;
    int interactionEnterTweenID;

    //================================================================================

    private void Start()
    {
        LeanTween.scale(area, new Vector3(0.3f, 2f, 0.3f), 0);
    }

    //================================================================================

    void PlayerEnter()
    {
        LeanTween.cancel(area);
        LeanTween.scale(area, new Vector3(2f, 2f, 2f), 0.2f).setEaseInOutQuad();

        onEnter?.Invoke();
    }
    void PlayerExit()
    {
        LeanTween.cancel(area);
        LeanTween.scale(area, new Vector3(0.3f, 2f, 0.3f), 0.1f).setEaseInOutQuad();

        onExit?.Invoke();
    }

    public void SetReady(bool _on, bool _immediate)
    {
        LeanTween.cancel(readyTweenID);
        readyTweenID = LeanTween.moveLocalY(gameObject, _on ? 0.001f : -2f, _immediate ? 0f : 0.4f).setEaseOutQuad().setOnComplete(() =>
        {
            if (interiorCol != null) interiorCol.enabled = _on;
        }).uniqueId;
    }

    public void SetColor(bool _white)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_Color", _white ? Color.white : Color.green);
        areaRend.SetPropertyBlock(block);
    }

    public void CancelInteractionLoad()
    {
        LeanTween.cancel(interactionEnterTweenID);
        interactionLoadImg.fillAmount = 0;
        LeanTween.cancel(loadImgAlpha.gameObject);
        loadImgAlpha.alpha = 0f;
    }
    public void InteractionLoad(float _loadTime, System.Action _callback)
    {
        interactionEnterTweenID = LeanTween.value(0, 1, _loadTime).setEaseOutQuad().setOnUpdate((float x) =>
            {
                loadImgAlpha.alpha = x * 2;
                interactionLoadImg.fillAmount = x;
            }).setOnComplete(() =>
            {
                CancelInteractionLoad();
                _callback?.Invoke();
            }).uniqueId;
    }

    //================================================================================

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerEnter();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerExit();
        }
    }

}