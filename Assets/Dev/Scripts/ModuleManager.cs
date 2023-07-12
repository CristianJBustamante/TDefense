using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModuleManager : MonoBehaviour
{

    public static ModuleManager instance;
    public delegate void OnUnlockBuilding();
    public static OnUnlockBuilding onUnlockBuilding;

    void Awake(){
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void CheckModules(){
        onUnlockBuilding.Invoke();
    }
}
