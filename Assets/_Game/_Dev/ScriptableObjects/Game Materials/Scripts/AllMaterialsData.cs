using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

[CreateAssetMenu(fileName = "AllMaterialsData", menuName = "Data/AllMaterialsData")]
public class AllMaterialsData : ScriptableObject
{
    public GameMaterial[] materials;
}
