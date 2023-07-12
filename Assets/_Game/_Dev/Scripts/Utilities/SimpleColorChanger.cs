using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleColorChanger : MonoBehaviour
{

    [Header("//COMPONENTS----------------------------")]
    [SerializeField] Renderer[] rend;

    [Space(20)]

    [Header("//PROPERTIES----------------------------")]
    [SerializeField] Color[] color;
    [SerializeField] Color[] emission;
    [Range(0, 1)] [SerializeField] float[] metallic;
    [Range(0, 1)] [SerializeField] float[] smoothness;
    [SerializeField] Texture[] texture;


    MaterialPropertyBlock propertyBlock;

    //--------------------------------------------------------------------------------

    private void Awake()
    {
        ChangeMaterialProperties();
    }

    private void OnValidate()
    {
        ChangeMaterialProperties();
    }

    //--------------------------------------------------------------------------------

    void ChangeMaterialProperties()
    {
        if (propertyBlock == null) propertyBlock = new MaterialPropertyBlock();
        if (rend == null) return;

        for (int i = 0; i < rend.Length; i++)
        {
            if (color.Length > i) propertyBlock.SetColor("_Color", color[i]);
            if (emission.Length > i) propertyBlock.SetColor("_Emission", emission[i]);
            if (metallic.Length > i) propertyBlock.SetFloat("_Metallic", metallic[i]);
            if (smoothness.Length > i) propertyBlock.SetFloat("_Smoothness", smoothness[i]);
            if (texture.Length > i) propertyBlock.SetTexture("_MainTex", texture[i]);
            if (rend[i] != null) rend[i].SetPropertyBlock(propertyBlock);
        }
    }

}
