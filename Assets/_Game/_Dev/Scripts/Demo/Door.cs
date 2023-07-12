using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {
    [SerializeField] Key key;
    [SerializeField] float rotationAngle;

    public void Interact() {
        if (key == null) return;

        if (key.State == Item.ItemState.OnInventory) {
            Open();
            Inventory.Instance.RemoveItem(key);
        }
    }

    void Open() => LeanTween.rotateLocal(gameObject, new Vector3(0, rotationAngle, 0), 1f);
}
