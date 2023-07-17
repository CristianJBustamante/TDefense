using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;
using com.Pizia.TouchJoypadSystem;

public class Movement : CachedReferences
{
    [Header("Properties")]
    public float speed;
    public float maxSpeed;
    public float rotSpeed;

    [Header("Info")]
    [SerializeField] float currentRotSpeed;
    [SerializeField] float currentSpeed;

    [Header("Components")]
    public Character character;

    float mxsp;

    [Header("Targeting")]
    
    public Transform TransformTarget;

    //=====================================================================================

#if UNITY_EDITOR
    private void Awake()
    {
        mxsp = speed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            speed = mxsp * 4;
        }
        else
        {
            speed = mxsp;
        }
    }
#endif

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    //=====================================================================================

    void Move()
    {
        if (character.rb.isKinematic) return;

        if (!character.GetJoyActive())
        {
            character.rb.velocity *= 0;
            return;
        }

        Vector3 moveDirection = new Vector3(character.GetJoyL().x, 0f, character.GetJoyL().y);
        character.rb.AddForce(moveDirection * currentSpeed);
        Vector3 clampedVelocity = character.rb.velocity.VectorClamp(new Vector3(1, 0, 1), -maxSpeed, maxSpeed);
        character.rb.velocity = new Vector3(clampedVelocity.x, character.rb.velocity.y, clampedVelocity.z);
    }

    void Rotate()
    {
        if (character.rb.isKinematic) return;

        currentSpeed = speed;
        currentRotSpeed = rotSpeed;

        Vector3 lookRotation = new Vector3(character.GetJoyL().x, 0f, character.GetJoyL().y);
        if (lookRotation.magnitude < Mathf.Epsilon) return;

        Quaternion rot = Quaternion.LookRotation(lookRotation, Vector3.up);
        character.rb.rotation = Quaternion.Slerp(character.rb.rotation, rot, currentRotSpeed * Time.fixedDeltaTime);
    }
}
