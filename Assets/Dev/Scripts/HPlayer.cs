using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;
using com.Pizia.Saver;

public class HPlayer : CachedReferences
{
    public static HPlayer instance;
    public SimpleMovement simpleMovement;
    public List<IAEnemy> iAEnemies;
    public IAEnemy nearEnemy;

    void Awake(){
        if(instance == null) instance = this;
        else Destroy(this);
    }

    public float detectionDistance = 10f;

    void Update()
    {
        iAEnemies.Clear();
        Collider[] collidersDetected = Physics.OverlapSphere(transform.position, detectionDistance);

        foreach (Collider c in collidersDetected)
        {
            if(c.TryGetComponent<IAEnemy>(out IAEnemy iAEnemy)) iAEnemies.Add(iAEnemy);
        }

        float distance = 1000;
        if(iAEnemies.Count > 0){
            foreach (IAEnemy e in iAEnemies)
            {
                float d = Vector3.Distance(e.transform.position, transform.position);
                if (d < distance){
                    distance = d;
                    nearEnemy = e;
                }
            }
            simpleMovement.transformEnemyTarget = nearEnemy.transform;
        }else{
            simpleMovement.transformEnemyTarget = null;
            nearEnemy = null;
        }

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}
