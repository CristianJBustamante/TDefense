using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class Character : CachedReferences
{
    [Header("Components")]
    public Rigidbody rb;
    public Collider col;
    public Animator anim;
    public GameObject model;
    public ToolManager toolManager;

    [Header("Debug Info")]
    public bool alive;
    public bool walking;

    public Movement Movement { get; private set; }


    //==================================================================================

    protected virtual void Awake()
    {
        Movement = GetComponent<Movement>();
    }

    protected virtual void Update()
    {
        walking = rb.velocity.magnitude > 0.5f;
        ManageAnimations();
    }

    //==================================================================================

    protected virtual void ManageAnimations()
    {
        if (!rb.isKinematic) anim.SetBool("Walk", walking);
    }

    public virtual bool GetJoyActive()
    {
        return true;
    }

    public virtual Vector2 GetJoyL()
    {
        return Vector2.zero;
    }
}
