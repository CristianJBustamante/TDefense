using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerModule : MonoBehaviour
{
    public Module module;
    public bool hasNeighbour;

    private void OnTriggerEnter(Collider other)
    {
        if (module != null) return;
        if (other.TryGetComponent<Module>(out module) )
        {
            hasNeighbour = true;
        }
    }
}
