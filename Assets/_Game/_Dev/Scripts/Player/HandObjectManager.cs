using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjectManager : MonoBehaviour
{
    [Header("Debug Info")]
    public InventoryTool activeTool;

    [Header("Components")]
    public Character character;
    public HandTool handTool;

    InteractionManager.InteractionType prevInteraction;

    //================================================================================

    void Start()
    {

    }

    //================================================================================

    public void SetActiveTool(InteractionManager.InteractionType _type)
    {
        if (_type == InteractionManager.InteractionType.None)
        {
            activeTool = null;
            StopAllCoroutines();
            StartCoroutine(CheckToRemoveObject(false));
            return;
        }

        activeTool = character.toolManager.GetInventoryTool(_type);
        handTool.SetTool(activeTool);
        handTool.Animate(true);

        StopAllCoroutines();
        StartCoroutine(CheckToRemoveObject());
        prevInteraction = _type;
    }

    //This is called when the animation is completed
    IEnumerator CheckToRemoveObject(bool waitForAnimation = true)
    {
        //Wait until playing the cut animation
        if (waitForAnimation)
        {
            while (character.anim.GetCurrentAnimatorClipInfo(1).Length == 0)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        //Once the animation is running, enable the collider
        handTool.SetCollider(true);

        while (character.anim.GetCurrentAnimatorClipInfo(1).Length > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        //Once completed, remove the hand object
        handTool.Animate(false);
        handTool.SetCollider(false);
    }
}
