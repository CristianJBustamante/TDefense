using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.TouchJoypadSystem;
using com.Pizia.Tools;

public class SimpleMovement : CachedReferences
{
    [Header("Properties")]
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    [Header("//Components--------------------------------//")]
    public Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform cameraTransform;
    [SerializeField] ParticleSystem dustPS;
    [SerializeField] Collider col;

    public Vector3 Velocity { get; private set; }

    TouchJoypad joy;
    Quaternion forwardRotation;
    bool acting;

    void Awake()
    {
        if (TouchJoypadManager.Instance == null) return;
        joy = TouchJoypadManager.Instance.GetJoypad(0);
    }

    void OnEnable()
    {
        if (joy == null) return;
        joy.joypadEvent += Direction;
        joy.joypadReleased += Stop;
    }

    void OnDisable()
    {
        if (joy == null) return;
        joy.joypadEvent -= Direction;
        joy.joypadReleased -= Stop;
    }

    void Update()
    {
        if (acting) return;
        if (animator != null) animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        if (dustPS == null) return;
        if (rb.velocity.sqrMagnitude >= 1)
        {
            if (!dustPS.isPlaying) dustPS.Play();
        }
        else
        {
            if (dustPS.isPlaying) dustPS.Stop();
        }
    }

    void FixedUpdate()
    {
        if (acting) return;
        MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, forwardRotation, Time.deltaTime * rotationSpeed);
        rb.velocity = new Vector3(Velocity.x, rb.velocity.y, Velocity.z);
    }

    void Direction(TouchJoypad sender, Vector2 dir)
    {
        if (acting) return;
        Vector3 actualDirection = new Vector3(dir.x, 0, dir.y);
        forwardRotation = Quaternion.LookRotation(actualDirection, Vector3.up);
        Velocity = actualDirection.normalized * speed;
    }

    void Stop(TouchJoypad sender) => Velocity = Vector3.zero;

    public void MovePlayerToDestination(Transform _target, bool _rotateOnFinish = false, float _moveTime = 0.8f)
    {
        acting = true;
        col.enabled = false;
        rb.isKinematic = true;
        animator.SetFloat("Speed", 10);

        if (!_rotateOnFinish)
        {
            LeanTween.move(gameObject, _target, _moveTime);
            LeanTween.rotateY(gameObject, _target.eulerAngles.y, 0.3f).setEaseInOutQuad();
        }
        else
        {
            transform.forward = (_target.position - transform.position);
            LeanTween.move(gameObject, _target, _moveTime).setOnComplete(() => {
                LeanTween.rotateY(gameObject, _target.eulerAngles.y, 0.2f).setEaseInOutQuad();
                animator.SetFloat("Speed", 0);
            });
        }
    }

    public void Toggle(bool toggle)
    {
        acting = !toggle;

        col.enabled = true;
        rb.isKinematic = false;
        rb.velocity = Velocity = Vector3.zero;
        animator.SetFloat("Speed", 0);

        if (toggle)
        {
            TouchJoypadManager.Instance.Enable();
        }
        else
        {
            TouchJoypadManager.Instance.Disable();
            dustPS.Stop();
        }
    }
}
