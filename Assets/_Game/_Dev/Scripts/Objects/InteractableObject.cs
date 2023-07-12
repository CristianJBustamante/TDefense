using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public int level = 1;
    public InteractionManager.InteractionType interaction;
    public ItemDrawFeel itemDrawFeel;
    public FarmableMaterial farmableMaterial;
    public Enemy enemy;
}
