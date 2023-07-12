using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.Pizia.Tools;
using com.Pizia.Saver;
using TMPro;

public class Player : Character
{
    [Header("Player Info")]
    public int playerLevel;
    public float maxHp;

    [Header("Player Components")]
    public InputManager inputManager;
    public InteractionManager interactionManager;
    public GameObject colliders;
    // public CanvasGroup hpAlpha;
    // public Image hpFill, hpFill2;
    // public Material normalMat, hitMat, hiddenMat;
    // public Renderer[] rends;
    public AnimationCurve hurtCurve;//, hpShakeCurve;

    [Header("Player Debug Info")]
    public Enemy currentEnemy;
    public float currentHp;

    string playerLevelKey = "Player Level";
    string playerSpawnKey = "Player Spawn";
    int hpAlphaTweenID, hpSecondFillID, hurtTweenID, hpShakeTweenID, matTweenID;
    float secondFillAmount;
    float hurtCD;
    public Vector3 respawnPos;

    Coroutine regenHpCoroutine;

    //==================================================================================

    protected override void Awake()
    {
        base.Awake();

        playerLevel = SaveManager.LoadInt(playerLevelKey);
        if (playerLevel == 0) playerLevel = 1;

        if (SaveManager.HasKey(playerSpawnKey))
        {
            respawnPos = SaveManager.LoadVector3(playerSpawnKey);
        }

        currentHp = secondFillAmount = maxHp;

        alive = true;
    }

    protected override void Update()
    {
        base.Update();

        // ManageHP();
    }

    //==================================================================================

    protected override void ManageAnimations()
    {
        base.ManageAnimations();
    }

    public override Vector2 GetJoyL()
    {
        return inputManager.GetJoyL();
    }

    public override bool GetJoyActive()
    {
        return inputManager.active;
    }

    public void LevelUp()
    {
        playerLevel++;
        SaveManager.SaveInt(playerLevelKey, playerLevel);
    }

    //====================================================================================

    public void Respawn()
    {
        GameManager.instance.TogglePlayerControl(true);

        rb.velocity *= 0;
        rb.isKinematic = false;
        model.SetActive(true);
        colliders.SetActive(true);
        // hpAlpha.gameObject.SetActive(true);
        currentHp = secondFillAmount = maxHp;

        transform.position = respawnPos;

        alive = true;
    }

    // public void ShowLevel(bool _on)
    // {
    //     LeanTween.cancel(hpAlphaTweenID);
    //     hpAlphaTweenID = LeanTween.value(_on ? 0f : 1f, _on ? 1f : 0f, 0.3f).setEaseOutQuad().setOnUpdate((float x) =>
    //     {
    //         hpAlpha.alpha = x;
    //     }).uniqueId;
    //     levelTxt.text = "Lv." + toolManager.GetInventoryTool(InteractionManager.InteractionType.Attack).level;
    //     levelTxt.gameObject.SetActive(_on);
    // }

    public void Hurt(Enemy _enemy)
    {
        if (hurtCD > Time.time) return;
        hurtCD = Time.time + 0.3f;

        if (regenHpCoroutine != null) StopCoroutine(regenHpCoroutine);
        regenHpCoroutine = StartCoroutine(RegenHPCoroutine());

        LeanTween.cancel(hurtTweenID);
        hurtTweenID = LeanTween.scale(model, Vector3.one * 0.8f, 0.3f).setEase(hurtCurve).uniqueId;

        currentHp -= _enemy.damage;
        LeanTween.cancel(hpSecondFillID);
        hpSecondFillID = LeanTween.delayedCall(0.3f, () => secondFillAmount = currentHp).uniqueId;

        // LeanTween.cancel(hpShakeTweenID);
        // hpShakeTweenID = LeanTween.moveX(hpAlpha.GetComponent<RectTransform>(), 0.05f, 0.6f).setEase(hpShakeCurve).uniqueId;

        // hurtPs.Play();

        // LeanTween.cancel(matTweenID);
        // for (int i = 0; i < rends.Length; i++)
        // {
        //     rends[i].sharedMaterial = hitMat;
        // }
        // matTweenID = LeanTween.delayedCall(0.2f, () =>
        // {
        //     for (int i = 0; i < rends.Length; i++)
        //     {
        //         rends[i].sharedMaterial = normalMat;
        //     }
        // }).uniqueId;

        // GameManager.instance.cameraManager.Shake(3);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.instance.TogglePlayerControl(false);

        // diePs.Play();
        rb.velocity *= 0;
        rb.isKinematic = true;
        model.SetActive(false);
        colliders.SetActive(false);
        // hpAlpha.gameObject.SetActive(false);
        // runPs.gameObject.SetActive(false);

        Debug.Log("Player killed by: " + currentEnemy.enemyName);
        // HomaBelly.Instance.TrackProgressionEvent(ProgressionStatus.Fail, "Player killed by: " + currentEnemy.enemyName);

        alive = false;

        LeanTween.delayedCall(2f, Respawn);
    }

    // public void SetHidden(bool _on)
    // {
    //     hiddenMat.SetColor("_BaseColor", new Color(1, 1, 1, _on ? 1 : 0));
    // }

    public void SetNewRespawn(Vector3 _pos)
    {
        respawnPos = _pos;
        SaveManager.SaveVector3(playerSpawnKey, respawnPos);
    }

    //==================================================================================

    // void ManageHP()
    // {
    //     hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount, PiziaUtilities.Map(currentHp, 0f, maxHp, 0f, 1f), Time.deltaTime * 10);
    //     hpFill2.fillAmount = Mathf.Lerp(hpFill2.fillAmount, PiziaUtilities.Map(secondFillAmount, 0f, maxHp, 0f, 1f), Time.deltaTime * 10);
    // }

    IEnumerator RegenHPCoroutine()
    {
        yield return new WaitForSeconds(5f);

        currentHp = secondFillAmount = maxHp;

        regenHpCoroutine = null;
    }

    //==================================================================================

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("EnemyDamage"))
        // {
        //     currentEnemy = other.GetComponentInParent<Enemy>();
        //     Hurt(currentEnemy);
        // }
    }
}
