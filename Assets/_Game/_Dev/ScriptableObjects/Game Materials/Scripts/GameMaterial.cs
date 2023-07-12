using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Pizia/GameMaterial")]
public class GameMaterial : ScriptableObject
{
    public enum MaterialType { Mineral, Plant, Meat }

    public MaterialType materialType;
    public UniqueID uid;
    public string materialName;
    public Mesh mesh;
    public Sprite icon;
    public GameObject prefab;
}