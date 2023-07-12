using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTool : MonoBehaviour
{
    [Header("Debug Info")]
    public InventoryTool tool;

    [Header("Components")]
    public InteractionManager interactionManager;
    public GameObject toolObj;
    public MeshFilter toolRend;
    [SerializeField] BoxCollider toolHitCol;
    [SerializeField] TrailRenderer trail;

    //===========================================================================

    private void Start()
    {
        toolObj.transform.localScale *= 0f;
    }

    //===========================================================================

    public void SetTool(InventoryTool _tool)
    {
        tool = _tool;

        toolRend.sharedMesh = tool.GetCurrentTool().mesh;

        switch (tool.GetCurrentTool().toolAction)
        {
            case ToolData.ToolAction.Mine:
            case ToolData.ToolAction.Chop:
            case ToolData.ToolAction.Attack:
                toolHitCol.enabled = true;
                toolHitCol.center = tool.GetCurrentTool().colliderCenter;
                toolHitCol.size = tool.GetCurrentTool().colliderSize;
                break;
        }

        trail.colorGradient = tool.GetCurrentTool().trailGradient;
    }

    public void Animate(bool _on)
    {
        LeanTween.cancel(toolObj);
        LeanTween.scale(toolObj, Vector3.one * (_on ? 0.01f : 0), 0.3f).setEaseOutQuad();
    }

    public void SetCollider(bool _on)
    {
        toolHitCol.enabled = _on;
    }
}
