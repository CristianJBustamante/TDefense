using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour {

    [SerializeField] Animator cameraState;

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Interactable"))
            collision.collider.GetComponent<IInteractable>().Interact();

        if (collision.collider.CompareTag("Item"))
            Inventory.Instance.AddItem(collision.collider.GetComponent<Item>());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Interactable"))
            other.GetComponent<IInteractable>().Interact();

        if (other.CompareTag("Item"))
            Inventory.Instance.AddItem(other.GetComponent<Item>());

        if (other.CompareTag("Finish"))
            cameraState.SetTrigger("Room 2");
    }

}
