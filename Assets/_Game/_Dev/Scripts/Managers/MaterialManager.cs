using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Saver;
using TMPro;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;
    public Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    [SerializeField] AllMaterialsData allMaterials;
    public List<InventoryItem> visualInventory = new List<InventoryItem>();

    [Header("UI Components")]
    public UIMaterial uiMaterialPrefab;
    public RectTransform uiMaterialsPivot;
    public List<UIMaterial> uiMaterials = new List<UIMaterial>();

    [Header("Farm +1 Components")]
    public int farmTextPool;
    public FarmText farmTextPrefab;
    public List<FarmText> farmTexts = new List<FarmText>();

    int txtCount;


    //=====================================================================================================

    public void Awake()
    {
        instance = this;

        //Populate items
        for (int i = 0; i < allMaterials.materials.Length; i++)
        {
            InventoryItem item = new InventoryItem();
            item.relatedMaterial = allMaterials.materials[i];

            item.firstTime = true;
            inventory.Add(allMaterials.materials[i].name, item);
            visualInventory.Add(item);

            uiMaterials.Add(Instantiate(uiMaterialPrefab, uiMaterialsPivot));
        }

        // farm texts
        for (int i = 0; i < farmTextPool; i++)
        {
            farmTexts.Add(Instantiate(farmTextPrefab, transform));
        }
    }

    private void Start()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Modify(allMaterials.materials[i], SaveManager.LoadInt(allMaterials.materials[i].uid), false, null, true);
            visualInventory[i].firstTime = !SaveManager.HasKey(allMaterials.materials[i].uid + "first time");
        }
        for (int i = 0; i < uiMaterials.Count; i++)
        {
            uiMaterials[i].Initialize(visualInventory[i].relatedMaterial.icon, visualInventory[i].amount);
            uiMaterials[i].gameObject.SetActive(visualInventory[i].amount > 0);
        }
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) AddResources();

        for (int i = 0; i < inventory.Count; i++)
        {
            visualInventory[i] = inventory[allMaterials.materials[i].name];
        }
#endif

        for (int i = 0; i < uiMaterials.Count; i++)
        {
            uiMaterials[i].UpdateData(visualInventory[i].amount);
            uiMaterials[i].gameObject.SetActive(visualInventory[i].amount > 0);
        }
    }

    //=====================================================================================================

    public bool Modify(GameMaterial targetMaterial, int amount = 1, bool _text = false, Transform _textParent = null, bool _avoidFirstTime = false)
    {
        inventory[targetMaterial.name].amount += amount;
        bool firstTime = inventory[targetMaterial.name].firstTime;
        if (firstTime && !_avoidFirstTime) inventory[targetMaterial.name].firstTime = false;

        SaveManager.SaveInt(targetMaterial.uid, inventory[targetMaterial.name].amount);
        if (!_avoidFirstTime) SaveManager.SaveBool(targetMaterial.uid + "first time", firstTime);

        if (_text) GenerateFarmText(amount, targetMaterial.icon, _textParent);

        return firstTime;
    }

    public bool Remove(GameMaterial targetMaterial, int amount = 1)
    {
        if (amount <= inventory[targetMaterial.name].amount)
        {
            inventory[targetMaterial.name].amount -= amount;

            SaveManager.SaveInt(targetMaterial.uid, inventory[targetMaterial.name].amount);

            return true;
        }
        return false;
    }

    public void ShowInfo(string _text, Sprite _icon, Transform _target)
    {
        for (int i = 0; i < farmTexts.Count; i++)
        {
            if (!farmTexts[i].animating)
            {
                farmTexts[i].Animate(_text, _icon, i, _target);
                break;
            }
        }
    }

    void GenerateFarmText(int _amount, Sprite _icon, Transform _textParent = null)
    {
        for (int i = 0; i < farmTexts.Count; i++)
        {
            if (!farmTexts[i].animating)
            {
                float yPos = 0;
                for (int j = 0; j < farmTexts.Count; j++)
                {
                    if (farmTexts[j] != farmTexts[i])
                    {
                        if (farmTexts[j].animating)
                        {
                            if (farmTexts[j].currentParent == _textParent)
                            {
                                yPos = farmTexts[j].transform.localPosition.y + 1f + txtCount;
                                txtCount++;
                                break;
                            }
                            else
                            {
                                txtCount = 0;
                            }
                        }
                    }
                    else
                    {
                        txtCount = 0;
                    }
                }
                farmTexts[i].Animate(_amount, _icon, yPos, _textParent);
                break;
            }
        }
    }

    [ConsoleFunction]
    public void AddResources()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Modify(allMaterials.materials[i], 500, false, null, true);
        }
    }

    //=====================================================================================================

    public Dictionary<string, InventoryItem> GetInventory()
    {
        return inventory;
    }

    public InventoryItem[] GetAllItemsOfType(GameMaterial.MaterialType _type)
    {
        List<InventoryItem> aux = new List<InventoryItem>();
        for (int i = 0; i < visualInventory.Count; i++)
        {
            if (visualInventory[i].relatedMaterial.materialType == _type)
            {
                aux.Add(visualInventory[i]);
            }
        }
        return aux.ToArray();
    }

    public InventoryItem GetInventoryItem(GameMaterial _material)
    {
        InventoryItem aux = null;
        for (int i = 0; i < visualInventory.Count; i++)
        {
            if (_material == visualInventory[i].relatedMaterial)
            {
                aux = visualInventory[i];
                break;
            }
        }
        return aux;
    }
}


[System.Serializable]
public class InventoryItem
{
    public GameMaterial relatedMaterial;
    public int amount;
    public bool firstTime = true;
}