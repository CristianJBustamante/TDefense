using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FarmableMaterial : MonoBehaviour
{
    [Header("Properties")]
    public int health;
    public bool canRespawn = true;
    public float delayToRespawn;
    public FarmableModelReplace[] modelReplaces;

    [Header("Components")]
    public MeshFilter targetMesh;
    public MeshRenderer rend;
    public Material hitMaterial;
    public ParticleSystem hitPS;
    public Collider col;

    public AnimationCurve scaleCurve, rotateCurve;

    private int startHealth;
    private Vector3 startSize;
    Material defaultMaterial;

    int scaleTweenID, rotateTweenID, materialTweenID;

    public delegate void OnDead();
    public OnDead onDead;

    //=====================================================================================

    void Start()
    {
        defaultMaterial = rend.material;
        startHealth = health;
        startSize = targetMesh.transform.localScale;
        if (!canRespawn) CheckDead();
    }

    //=====================================================================================

    void CheckDead()
    {
        // health = LevelManager.instance.saveManager.CheckIfKeyExist(specialName) ? LevelManager.instance.saveManager.LoadInt(specialName) : health;
        for (int i = 0; i < modelReplaces.Length; i++)
        {
            if (health <= modelReplaces[i].targetLife)
            {
                targetMesh.mesh = modelReplaces[i].newMesh;
            }
        }
        col.enabled = health > 1;
    }

    public void ReceivedHit(int _damage)
    {
        if (_damage == 0)
        {
            _damage = 1;
        }

        health--;
        if (health < 1) health = 1;
        for (int i = 0; i < modelReplaces.Length; i++)
        {
            if (health == modelReplaces[i].targetLife)
            {
                targetMesh.mesh = modelReplaces[i].newMesh;
                break;
            }
        }

        HitFeel();

        if (health <= 1)
        {
            Dead();
        }
        if (canRespawn)
        {
            CancelInvoke(nameof(Respawn));
            Invoke(nameof(Respawn), delayToRespawn);
        }
    }

    void HitFeel()
    {
        LeanTween.cancel(scaleTweenID);
        LeanTween.cancel(rotateTweenID);
        LeanTween.cancel(materialTweenID);

        float animTime = 0.3f;
        scaleTweenID = LeanTween.scale(targetMesh.gameObject, Vector3.one * 0.9f, animTime * 0.65f).setEase(scaleCurve).uniqueId;
        rotateTweenID = LeanTween.rotateAround(targetMesh.gameObject, Vector3.up, 15f, animTime).setEase(rotateCurve).uniqueId;

        rend.material = hitMaterial;
        materialTweenID = LeanTween.delayedCall(0.1f, () => { rend.material = defaultMaterial; }).uniqueId;

        // GameManager.instance.cameraManager.Shake(1);

        hitPS.Play();
    }

    void Dead()
    {
        col.enabled = false;
        onDead?.Invoke();
    }

    void Respawn()
    {
        health = startHealth;
        targetMesh.transform.localScale = Vector3.zero;
        LeanTween.scale(targetMesh.gameObject, startSize, 0.3f).setEaseOutBack();
        targetMesh.mesh = modelReplaces[0].newMesh;
        rend.material = defaultMaterial;
        col.enabled = true;
    }
}

[System.Serializable]
public class FarmableModelReplace
{
    public Mesh newMesh;
    public int targetLife;
}

