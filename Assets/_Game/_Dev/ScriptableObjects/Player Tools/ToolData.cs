using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

[CreateAssetMenu(fileName = "ToolData", menuName = "Data/ToolData")]
public class ToolData : ScriptableObject
{
    public enum ToolAction { Mine, Chop, Attack }

    public UniqueID uid;
    public string toolName;
    public bool startLocked;
    public ToolUpgrade[] upgrades;
}

[System.Serializable]
public class ToolUpgrade
{
    public ToolData.ToolAction toolAction;
    public string upgradeName;
    public int damage;
    public Sprite icon;
    public Mesh mesh;
    public Vector3 colliderCenter, colliderSize;
    public Gradient trailGradient;
}
