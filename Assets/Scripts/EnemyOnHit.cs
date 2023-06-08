using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnHit : MonoBehaviour
{
    public GameObject projectilePrefabRingOnDeath;
    public GameObject projectilePrefabOnDeath;
    private float projectileSpeed = 5.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponent<Faction>().IsFriendly(collision.GetComponentInParent<Faction>()) && !collision.isTrigger)
        {
            Destroy(collision);
            Destroy(gameObject);
            if (projectilePrefabOnDeath)
            {
                GetComponent<LaunchProjectile>().LaunchProjectileAtRandom(projectilePrefabOnDeath, projectileSpeed);
            }
            if (projectilePrefabRingOnDeath)
            {
                GetComponent<LaunchProjectile>().LaunchProjectileRing(projectilePrefabRingOnDeath, projectileSpeed, Random.Range(4, 9));
            }
        }
    }
}
