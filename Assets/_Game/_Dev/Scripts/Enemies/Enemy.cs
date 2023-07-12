using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Pizia.Tools;
using com.Pizia.Saver;
using TMPro;

public class Enemy : CachedReferences
{
    public enum EnemyState { Idle, Walk, Chase, Attack, Hurt, Dead }

    [Header("Properties")]
    public string enemyName;
    public int level;
    public float maxHp;
    public float speed;
    public float chaseRange, attackRange;
    public float damage;
    public bool patrol = true;
    public LayerMask worldLayerMask;

    [Header("Components")]
    public Rigidbody rb;
    public GameObject model;
    public Animator anim;
    public GameObject colliders, damageCol;
    public Image hpFill, hpFill2;
    public CanvasGroup hpAlpha;
    public ParticleSystem hurtPs, diePs;
    public TMP_Text levelTxt, level2Txt;
    public RectTransform warning;
    public Renderer[] rends;
    public Material normalMat, hitMat;
    public ItemDrawFeel itemDrawFeel;
    public AudioSource bonk;
    public AnimationCurve hurtCurve, hpShakeCurve;

    [Header("Debug Info")]
    public EnemyState state;
    public bool alive;
    public bool inChaseRange;
    public bool customDirection;
    public float currentHp;
    public float currentSpeed;
    public float currentRotationSpeed;

    [HideInInspector] public float speedValue, rotSpeedValue;
    float randomDirectionCounter;
    float maxSpeed;
    float checkEnvironmentCounter;
    float walkCounter, idleCounter;
    float secondFillAmount;
    float hurtCD;
    int hpAlphaTweenID, hurtTweenID, hpSecondFillID, hpShakeTweenID, matTweenID;
    Vector3 startPos;
    Vector3 randomForward;
    Vector3 deadPsPos, deadPsRot;

    public delegate void OnDead(Enemy _enemy);
    public OnDead onDead;

    Player player => GameManager.instance.playerCharacter;

    //====================================================================

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, chaseRange);
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }

    private void Awake()
    {
        startPos = transform.position;
        hpAlpha.alpha = 0f;
        maxSpeed = speed;
        speedValue = speed;
        currentHp = secondFillAmount = maxHp;
        deadPsPos = diePs.transform.localPosition;
        deadPsRot = diePs.transform.localEulerAngles;
        Spawn(transform.position);
    }

    private void Update()
    {
        ManageHP();
    }

    private void FixedUpdate()
    {
        EnemyStateManagement();
        Move();
    }

    //====================================================================

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        Spawn(startPos);
    }
    public void Spawn(Vector3 _position)
    {
        transform.position = _position;
        rb.isKinematic = false;
        model.SetActive(true);
        colliders.SetActive(true);
        hpAlpha.gameObject.SetActive(true);
        transform.localScale = Vector3.one;
        hpAlpha.alpha = 0f;
        maxSpeed = speed;
        speedValue = speed;
        currentHp = secondFillAmount = maxHp;
        LeanTween.delayedCall(1f, () =>
        {
            diePs.transform.parent = transform;
            diePs.transform.localPosition = deadPsPos;
            diePs.transform.localEulerAngles = deadPsRot;
        });
        alive = true;
    }
    [ContextMenu("Despawn")]
    public void Despawn()
    {
        rb.velocity *= 0;
        rb.isKinematic = true;
        model.SetActive(false);
        colliders.SetActive(false);
        damageCol.SetActive(false);
        inChaseRange = false;
        alive = false;
    }

    public void Hurt(float _damage)
    {
        if (hurtCD > Time.time) return;
        hurtCD = Time.time + 0.3f;

        LeanTween.cancel(hurtTweenID);
        hurtTweenID = LeanTween.scaleY(model, 0.3f, 0.3f).setEase(hurtCurve).uniqueId;

        currentHp -= _damage;
        LeanTween.cancel(hpSecondFillID);
        hpSecondFillID = LeanTween.delayedCall(0.3f, () => secondFillAmount = currentHp).uniqueId;

        LeanTween.cancel(hpShakeTweenID);
        hpShakeTweenID = LeanTween.moveX(hpAlpha.GetComponent<RectTransform>(), 0.05f, 0.6f).setEase(hpShakeCurve).uniqueId;

        hurtPs.Play();
        bonk.Play();

        LeanTween.cancel(matTweenID);
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].sharedMaterial = hitMat;
        }
        matTweenID = LeanTween.delayedCall(0.2f, () =>
        {
            for (int i = 0; i < rends.Length; i++)
            {
                rends[i].sharedMaterial = normalMat;
            }
        }).uniqueId;

        // GameManager.instance.cameraManager.Shake(2);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        rb.velocity *= 0;
        rb.isKinematic = true;
        model.SetActive(false);
        colliders.SetActive(false);
        damageCol.SetActive(false);
        inChaseRange = false;
        hpAlpha.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            itemDrawFeel.InstanceOne();
        }
        itemDrawFeel.FarmOne(Random.Range(2, 3), true, transform);

        diePs.transform.parent = null;
        diePs.Play();

        alive = false;

        onDead?.Invoke(this);
    }

    //====================================================================

    void EnemyStateManagement()
    {
        if (!alive) return;

        anim.SetBool("Walk", rb.velocity.magnitude > 0.1f);
        Vector3 newForward = Vector3.zero;
        switch (state)
        {
            case EnemyState.Idle:
                CheckChase();
                speedValue = 0;

                if (patrol && idleCounter < Time.time)
                {
                    ChangeState(EnemyState.Walk);
                    walkCounter = Time.time + Random.Range(2f, 5f);
                }
                break;
            case EnemyState.Walk:
                CheckChase();
                if (patrol)
                {
                    speedValue = speed;
                    if (randomDirectionCounter < Time.time)
                    {
                        if (customDirection) return;
                        newForward = PiziaUtilities.GenerateRandomVector(new Vector3(1f, 0f, 1f), -1f, 1f);
                        SetDirection(newForward, new Vector2(4f, 7f), true);
                    }
                    rotSpeedValue = 3f;
                    CheckEnvironment();
                    Direction(newForward);

                    if (walkCounter < Time.time)
                    {
                        ChangeState(EnemyState.Idle);
                        anim.SetTrigger(Random.Range(0, 2) > 0 ? "Idle" : "Eat");
                        idleCounter = Time.time + Random.Range(2f, 5f);
                    }
                }
                break;
            case EnemyState.Chase:
                speedValue = speed;
                newForward = (player.rb.position - rb.position).normalized;
                newForward.y = 0f;
                SetDirection(newForward, Vector2.zero, false);
                rotSpeedValue = 10f;
                Direction(newForward);

                if (Vector3.Distance(rb.position, player.rb.position) < attackRange)
                {
                    ChangeState(EnemyState.Attack);
                    anim.SetTrigger("Attack");
                    ChangeState(EnemyState.Chase, anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
                }
                else if (Vector3.Distance(rb.position, player.rb.position) > chaseRange || !player.alive)
                {
                    ShowHP(false);
                    anim.SetTrigger("Idle");
                    anim.ResetTrigger("Attack");
                    ChangeState(EnemyState.Idle);
                }
                break;
            case EnemyState.Attack:
                speedValue = 0;
                if (!player.alive)
                {
                    ShowHP(false);
                    anim.SetTrigger("Idle");
                    anim.ResetTrigger("Attack");
                    ChangeState(EnemyState.Idle);
                }
                break;
            case EnemyState.Hurt:
                speedValue = 0;
                break;
            case EnemyState.Dead:
                speedValue = 0;
                alive = false;
                break;
        }
    }

    void Move()
    {
        if (!alive) return;

        currentSpeed = Mathf.Lerp(currentSpeed, speedValue, Time.deltaTime * 6f);
        rb.velocity = transform.forward * currentSpeed;
    }

    void Direction(Vector3 _direction)
    {
        if (!alive) return;

        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, rotSpeedValue, Time.deltaTime * 9f);
        if (randomForward.magnitude == 0f) randomForward = PiziaUtilities.GenerateRandomVector(new Vector3(1f, 0f, 1f), -1f, 1f);
        Quaternion lookRotation = Quaternion.LookRotation(randomForward, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.deltaTime * currentRotationSpeed);
    }

    void CheckEnvironment()
    {
        if (!alive) return;

        if (checkEnvironmentCounter < Time.time)
        {
            RaycastHit hit;
            Vector3 rayOrigin = new Vector3(transform.position.x, 1f, transform.position.z);
            float rayLength = 5f;
            if (Physics.Raycast(rayOrigin, transform.TransformDirection(Vector3.forward), out hit, rayLength, worldLayerMask))
            {
                if (hit.collider.transform.parent.parent == transform) return;

                Debug.DrawRay(rayOrigin, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                // turn around
                Vector2 timeRange = new Vector2(3f, 5f);
                SetDirection(-randomForward, timeRange, true);
                checkEnvironmentCounter = Time.time + 0.7f;
            }
            else
            {
                Debug.DrawRay(rayOrigin, transform.TransformDirection(Vector3.forward) * rayLength, Color.white);

                checkEnvironmentCounter = Time.time + 0.3f;
            }
        }
    }

    void CheckChase()
    {
        if (!alive) return;
        if (!player.alive) return;

        if (Vector3.Distance(rb.position, player.rb.position) < chaseRange)
        {
            ShowHP(true);
            ChangeState(EnemyState.Chase);
        }
    }

    void SetDirection(Vector3 _dir, Vector2 _timerRange, bool _resetTimer = false)
    {
        randomForward = _dir;
        if (_resetTimer) randomDirectionCounter = Time.time + Random.Range(_timerRange.x, _timerRange.y);
    }

    void ChangeState(EnemyState _state, float _delay = 0)
    {
        LeanTween.delayedCall(_delay, () =>
        {
            state = _state;
        });
    }

    void ShowHP(bool _on)
    {
        levelTxt.gameObject.SetActive(level <= player.toolManager.GetInventoryTool(InteractionManager.InteractionType.Attack).level);
        level2Txt.gameObject.SetActive(level > player.toolManager.GetInventoryTool(InteractionManager.InteractionType.Attack).level);

        LeanTween.cancel(hpAlphaTweenID);
        hpAlphaTweenID = LeanTween.value(_on ? 0f : 1f, _on ? 1f : 0f, 0.3f).setEaseOutQuad().setOnUpdate((float x) =>
        {
            hpAlpha.alpha = x;
        }).uniqueId;

        LeanTween.cancel(warning);
        warning.localScale *= 0;
        LeanTween.scale(warning, Vector3.one, 0.3f).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.scale(warning, Vector3.one * 1.4f, 0.35f).setEaseInOutQuad().setLoopPingPong();
        });
    }

    void ManageHP()
    {
        hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount, PiziaUtilities.Map(currentHp, 0f, maxHp, 0f, 1f), Time.deltaTime * 10);
        hpFill2.fillAmount = Mathf.Lerp(hpFill2.fillAmount, PiziaUtilities.Map(secondFillAmount, 0f, maxHp, 0f, 1f), Time.deltaTime * 10);

        if (levelTxt.isActiveAndEnabled) levelTxt.text = "Lv." + level;
        if (level2Txt.isActiveAndEnabled) level2Txt.text = "Lv." + level;
    }

    //==================================================================================

}
