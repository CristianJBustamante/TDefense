using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class HitDetector : CachedReferences
{
    public HandTool handTool;

    InteractableObject prevInteractable;
    float hitCD;

    //===========================================================================

    void OnTriggerEnter(Collider coll)
    {
        InteractableObject interactable = coll.GetComponent<InteractableObject>();
        if (interactable == null) interactable = coll.GetComponentInParent<InteractableObject>();

        if (interactable != null)
        {
            if (!GameManager.instance.playerCharacter.toolManager.CheckToolUnlocked(interactable.interaction)) return;
            if (interactable.interaction != handTool.tool.interactionType) return;

            if (prevInteractable == interactable)
            {
                if (hitCD > Time.time) return;
            }
            prevInteractable = interactable;
            if (handTool.tool.level >= interactable.level)
            {
                if (interactable.farmableMaterial != null)
                {
                    interactable.farmableMaterial.ReceivedHit(handTool.tool.GetCurrentTool().damage);
                }
                if (interactable.itemDrawFeel != null)
                {
                    interactable.itemDrawFeel.InstanceOne();
                    interactable.itemDrawFeel.FarmOne(1, true, interactable.transform);
                }
                if (interactable.enemy != null)
                {
                    if (interactable.enemy.level <= GameManager.instance.playerCharacter.toolManager.GetInventoryTool(interactable.interaction).level)
                    {
                        interactable.enemy.Hurt(handTool.tool.GetCurrentTool().damage);
                    }
                }
            }
            else
            {
                handTool.interactionManager.CutFailAnimation();
            }

            hitCD = Time.time + Time.deltaTime + 0.3f;
        }
    }
}
