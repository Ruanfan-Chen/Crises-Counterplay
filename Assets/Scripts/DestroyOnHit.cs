using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    public GameObject projectilePrefabRingOnDeath;
    public GameObject projectilePrefabOnDeath;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Faction>().GetHostility() != collision.GetComponentInParent<Faction>().GetHostility())
        {
            Vector3 position = transform.position;
            Destroy(collision);
            Destroy(gameObject);
            if (projectilePrefabOnDeath) {
                GetComponent<EnemyLaunch>().LaunchProjectile(projectilePrefabOnDeath);
            }
            if (projectilePrefabRingOnDeath) {
                GetComponent<EnemyLaunch>().LaunchProjectileRing(projectilePrefabRingOnDeath, Random.Range(4, 9));
            }
        }
    }


}
