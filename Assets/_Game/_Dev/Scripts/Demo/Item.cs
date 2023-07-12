using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{

    public ItemState State { get; private set; }

    public enum ItemState { OnScene, OnInventory}

    private void Awake()
    {
        ChangeState(ItemState.OnScene);
    }

    public void ChangeState(ItemState state)
    {
        State = state;

        switch (State)
        {
            case ItemState.OnInventory:
                LeanTween.scale(gameObject, Vector3.zero, 1f);
                GetComponent<Collider>().enabled = false;
                break;
            case ItemState.OnScene:
                LeanTween.scale(gameObject, Vector3.one * 0.1f, 1f);
                GetComponent<Collider>().enabled = true;
                break;     
        }
    }

}
