using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public enum InteractionType { None, Chop, Mine, Attack };

    [Header("Properties")]
    public Vector3 overlapBoxSize;
    public Vector3 overlapBoxOffset;
    public LayerMask layerMask;

    [Header("Debug Info")]
    public InteractionType currentInteraction;
    public bool mine;

    [Header("Components")]
    public HandTool handTool;
    public HandObjectManager handObjectManager;
    public Character character;

    public delegate void EventHandler(InteractionType newInteraction);
    public EventHandler OnInteractionChanged;

    private Collider previousColl;
    private InteractionType previousInteraction;
    Vector3 overlapBoxPos;
    float interactionTick;
    ToolManager toolManager => GameManager.instance.playerCharacter.toolManager;
    Transform interactionPivot;
    float interactionCounter;

    InteractableObject interactableObj;

    //================================================================================

    void Start()
    {
        previousInteraction = currentInteraction;
    }

    void Update()
    {
        CheckInteraction();
    }

    void OnDrawGizmosSelected()
    {
        SetOverlapBox();
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(overlapBoxPos, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = new Color(.5f, .5f, .5f, .2f);
        Gizmos.DrawCube(Vector3.zero, overlapBoxSize);
    }

    //================================================================================

    void CheckInteraction()
    {
        if (!character.alive)
        {
            HandleInteractions(InteractionType.None);
            return;
        }

        SetOverlapBox();
        Collider[] colls = Physics.OverlapBox(overlapBoxPos, overlapBoxSize / 2, transform.rotation, layerMask);

        if (colls.Length == 0)
        {
            currentInteraction = InteractionType.None;
            previousColl = null;
        }
        else
        {
            Collider selectedCol = colls[0];
            for (int i = 0; i < colls.Length; i++)
            {
                InteractableObject obj = colls[i].GetComponentInParent<InteractableObject>();
                if (obj.interaction == InteractionType.Attack)
                {
                    if (obj.enemy != null)
                    {
                        if (obj.enemy.level <= GameManager.instance.playerCharacter.toolManager.GetInventoryTool(obj.interaction).level)
                        {
                            selectedCol = colls[i];
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }

            if (previousColl != selectedCol)
            {
                interactableObj = selectedCol.GetComponentInParent<InteractableObject>();
                InteractionType newInteraction = interactableObj.interaction;
                interactionPivot = selectedCol.transform;

                if (interactableObj.enemy != null)
                {
                    if (interactableObj.enemy.level > GameManager.instance.playerCharacter.toolManager.GetInventoryTool(interactableObj.interaction).level)
                    {
                        return;
                    }
                }

                if (Vector3.Distance(interactableObj.transform.position, character.transform.position) > 3f)
                {
                    return;
                }

                currentInteraction = newInteraction;
                previousColl = selectedCol;
            }
        }

        if (previousInteraction != currentInteraction || interactionTick < Time.time)
        {
            OnInteractionChanged?.Invoke(currentInteraction);
            HandleInteractions(currentInteraction);
            previousInteraction = currentInteraction;

            interactionTick = Time.time + 0.1f;
        }
    }

    void SetOverlapBox()
    {
        overlapBoxPos = transform.position;
        overlapBoxPos += transform.forward * overlapBoxOffset.z;
        overlapBoxPos.y += overlapBoxOffset.y;
    }

    void HandleInteractions(InteractionType _newInteraction)
    {
        if (_newInteraction != InteractionType.None)
        {
            if ((!toolManager.CheckToolUnlocked(_newInteraction) || interactableObj.level > toolManager.GetInventoryTool(_newInteraction).level) && interactionPivot != null)
            {
                if (interactionCounter < Time.time)
                {
                    // MaterialManager.instance.ShowInfo("NEEDED", toolManager.GetInventoryTool(_newInteraction).toolData.upgrades[interactableObj.level - 1].icon, interactionPivot.transform);
                    Debug.LogError("Tool not unlocked!");
                }
                interactionCounter = Time.time + Time.deltaTime + 0.5f;
                return;
            }
        }

        switch (_newInteraction)
        {
            case InteractionType.None:
                mine = false;
                break;
            case InteractionType.Chop:
            case InteractionType.Mine:
            case InteractionType.Attack:
                mine = true;
                handObjectManager.SetActiveTool(_newInteraction);
                break;
        }

        character.anim.SetBool("Mine", mine);
    }

    //================================================================================

    public void CutFailAnimation()
    {
        character.anim.SetTrigger("fail");
    }
}
