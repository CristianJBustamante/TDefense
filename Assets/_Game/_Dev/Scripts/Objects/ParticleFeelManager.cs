using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFeelManager : MonoBehaviour
{
    public static ParticleFeelManager instance;

    public GameObject jumpParticlePrefab;
    public int jumpParticleCount;

    private List<MaterialParticle> jumpParticles;
    private Transform playerTransform;

    void Start()
    {
        instance = this;
        jumpParticles = new List<MaterialParticle>();
        playerTransform = GameManager.instance.playerCharacter.transform;
        for (int i = 0; i < jumpParticleCount; i++)
        {
            jumpParticles.Add(Instantiate(jumpParticlePrefab, transform).GetComponent<MaterialParticle>());
        }
    }

    public void ToPlayer(Vector3 startPos, GameMaterial material)
    {
        for (int i = 0; i < jumpParticles.Count; i++)
        {
            if (!jumpParticles[i].playing)
            {
                jumpParticles[i].Initialize(startPos, material.mesh, playerTransform);
                break;
            }
        }
    }

    public void ToPlayer(Vector3 startPos, Mesh _mesh)
    {
        for (int i = 0; i < jumpParticles.Count; i++)
        {
            if (!jumpParticles[i].playing)
            {
                jumpParticles[i].Initialize(startPos, _mesh, playerTransform);
                break;
            }
        }
    }

    public void FromPlayer(Vector3 targetPos, GameMaterial material)
    {
        for (int i = 0; i < jumpParticles.Count; i++)
        {
            if (!jumpParticles[i].playing)
            {
                jumpParticles[i].Initialize(playerTransform.position, material.mesh, targetPos);
                break;
            }
        }
    }

    public void FromPlayer(Vector3 targetPos, Mesh _mesh)
    {
        for (int i = 0; i < jumpParticles.Count; i++)
        {
            if (!jumpParticles[i].playing)
            {
                jumpParticles[i].Initialize(playerTransform.position, _mesh, targetPos);
                break;
            }
        }
    }
}
