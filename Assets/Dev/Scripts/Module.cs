using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;




public enum typeOfBuilding { None, House, Tower, Archery, Barracks, Castle }
public enum typeOfGround { Grass, Dust, Sand, Water }
public enum typeOfSurface { Flat, Route, Merge }
public enum ModuleState { Unreachable, Locked, Unlocked}

public class Module : MonoBehaviour
{

    [SerializeField] List<TypeOfGround> typeOfGrounds;
    [SerializeField] List<Building> buildings;
    [SerializeField] List<TriggerModule> triggerModules;
    [SerializeField] typeOfBuilding typeOfBuilding;
    [SerializeField] typeOfGround finalTypeOfGround;
    [SerializeField] typeOfGround initialTypeOfGround;
    [SerializeField] typeOfSurface typeOfSurface;
    [SerializeField] ModuleState moduleState;

    [SerializeField] GameObject myGround;
    [SerializeField] Building myBuilding;

    public ModuleState _ModuleState
    {
        get => moduleState;
        set
        {
            moduleState = value;
            switch (moduleState)
            {
                case ModuleState.Unreachable:
                    SetModel(initialTypeOfGround);
                    break;
                case ModuleState.Locked:
                    SetModel(finalTypeOfGround);
                    if (typeOfBuilding != typeOfBuilding.None)
                    {
                        foreach (Building b in buildings) if (b.buldingName == typeOfBuilding.ToString()) { b.gameObject.SetActive(true); break; }
                    }
                    break;
                case ModuleState.Unlocked:
                    SetModel(finalTypeOfGround);
                    LeanTween.delayedCall(0.2f, () =>
                    {
                        ModuleManager.instance.CheckModules();
                    });
                    break;
            }
        }
    }

    void Start()
    {
        // SetInitialConfiguration();
        _ModuleState = moduleState;
        foreach (Building b in buildings) if (b.name == typeOfBuilding.ToString()) myBuilding = b;

    }

    void OnEnable()
    {
        ModuleManager.onUnlockBuilding += CheckMyModuleForNeighbour;
    }

    void OnDisable()
    {
        ModuleManager.onUnlockBuilding -= CheckMyModuleForNeighbour;
    }

    [ContextMenu("Unlock Build")]
    public void UnlockBuild()
    {
        _ModuleState = ModuleState.Unlocked;
    }

    public void SetInitialConfiguration()
    {
        switch (moduleState)
        {
            case ModuleState.Unreachable:
                SetModel(initialTypeOfGround);
                break;
            case ModuleState.Locked:
                SetModel(finalTypeOfGround);
                break;
            case ModuleState.Unlocked:
                SetModel(finalTypeOfGround);
                break;

        }
    }

    public void SetModel(typeOfGround typeOfGround)
    {
        myGround.SetActive(false);
        switch (typeOfGround)
        {
            case typeOfGround.Grass:
                myGround = SearchTypeOfGround(typeOfGround.ToString()).SearchTypeOfModuleSurface("Simple").SearchTypeOfSurface("Flat").modelSurface;
                myGround.SetActive(true);
                break;
            case typeOfGround.Dust:
                myGround = SearchTypeOfGround(typeOfGround.ToString()).SearchTypeOfModuleSurface("Simple").SearchTypeOfSurface("Flat").modelSurface;
                myGround.SetActive(true);
                break;
            case typeOfGround.Sand:
                myGround = SearchTypeOfGround(typeOfGround.ToString()).SearchTypeOfModuleSurface("Simple").SearchTypeOfSurface("Flat").modelSurface;
                myGround.SetActive(true);
                break;
            case typeOfGround.Water:
                myGround = SearchTypeOfGround(typeOfGround.ToString()).SearchTypeOfModuleSurface("Simple").SearchTypeOfSurface("Flat").modelSurface;
                myGround.SetActive(true);
                break;

        }
    }

    TypeOfGround SearchTypeOfGround(string name)
    {
        foreach (TypeOfGround type in typeOfGrounds)
        {
            if (type.name == name) return type;
        }
        return null;
    }

    void CheckMyModuleForNeighbour()
    {
        if (_ModuleState != ModuleState.Unreachable) return;
        bool changed = false;
        foreach (TriggerModule tm in triggerModules)
        {
            if (changed) break;
            if (tm.hasNeighbour)
                if (tm.module.moduleState == ModuleState.Unlocked)
                {
                    changed = true;
                    break;
                }
                else
                {
                    foreach (TriggerModule tmt in tm.module.triggerModules)
                    {
                        if (tmt.hasNeighbour)
                            if (tmt.module.moduleState == ModuleState.Unlocked)
                            {
                                changed = true;
                                break;
                            }
                    }
                }
        }

        if (changed)
        {
            _ModuleState = ModuleState.Locked;
        }
    }

}



[System.Serializable]
public class TypeOfGround
{
    public string name;
    public List<TypeOfModuleSurface> modelSurfaces;

    public TypeOfModuleSurface SearchTypeOfModuleSurface(string name)
    {
        foreach (TypeOfModuleSurface type in modelSurfaces)
        {
            if (type.name == name) return type;
        }
        return null;
    }
}

[System.Serializable]
public class TypeOfModuleSurface
{
    public string name;
    public List<Surface> surfaces;

    public Surface SearchTypeOfSurface(string name)
    {
        foreach (Surface type in surfaces)
        {
            if (type.name == name) return type;
        }
        return null;
    }
}


[System.Serializable]
public class Surface
{
    public string name;
    public GameObject modelSurface;
}
