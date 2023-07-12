using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class ToolManager : CachedReferences
{
    public InventoryTool[] tools;

    //=========================================================================================

    private void Awake()
    {
        LoadData();
    }

    //=========================================================================================

    void LoadData()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].unlocked = SaveManager.HasKey(tools[i].uid) ? true : !tools[i].toolData.startLocked;
            tools[i].level = SaveManager.LoadInt(tools[i].uid + "level");
            if (tools[i].level == 0) tools[i].level = 1;
        }
    }

    public void UnlockTool(InteractionManager.InteractionType _interaction)
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].interactionType == _interaction)
            {
                tools[i].unlocked = true;
                SaveManager.SaveBool(tools[i].uid, true);
                break;
            }
        }
    }

    public void UpgradeTool(InteractionManager.InteractionType _interaction, int _level)
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].interactionType == _interaction)
            {
                if (tools[i].level < tools[i].toolData.upgrades.Length)
                {
                    tools[i].level = _level;
                    SaveManager.SaveInt(tools[i].uid + "level", tools[i].level);
                }
                else
                {
                    Debug.LogWarning("Max Level reached for " + tools[i].toolData.toolName);
                }
                break;
            }
        }
    }

    public bool CheckToolUnlocked(InteractionManager.InteractionType _interaction)
    {
        bool aux = false;
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].interactionType == _interaction)
            {
                aux = tools[i].unlocked;
                break;
            }
        }
        return aux;
    }

    public InventoryTool GetInventoryTool(InteractionManager.InteractionType _interaction)
    {
        InventoryTool aux = null;
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].interactionType == _interaction)
            {
                aux = tools[i];
                break;
            }
        }
        if (aux == null) Debug.LogError("Couldn't find a tool for that interaction (" + _interaction + ").");
        return aux;
    }
}

[System.Serializable]
public class InventoryTool
{
    public InteractionManager.InteractionType interactionType;
    public UniqueID uid;
    public bool unlocked;
    public int level;
    public ToolData toolData;

    public ToolUpgrade GetCurrentTool()
    {
        return toolData.upgrades[level - 1];
    }
}
