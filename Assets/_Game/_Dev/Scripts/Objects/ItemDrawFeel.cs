using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrawFeel : MonoBehaviour
{
    public GameMaterial material;

    public void InstanceOne()
    {
        for (int i = 0; i < Random.Range(1, 5); i++)
        {
            ParticleFeelManager.instance.ToPlayer(transform.position, material);
        }
    }

    public void FarmOne(int _amount = 1, bool _text = false, Transform _parent = null)
    {
        MaterialManager.instance.Modify(material, _amount, _text, _parent);
    }

    public void FarmOne()
    {
        MaterialManager.instance.Modify(material, 1);
    }
}
