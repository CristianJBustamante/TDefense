using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddItem(Item item)
    {
        item.transform.SetParent(transform);
        item.ChangeState(Item.ItemState.OnInventory);
    }

    public void RemoveItem(Item item)
    {
        Destroy(item.gameObject);
    }

}
