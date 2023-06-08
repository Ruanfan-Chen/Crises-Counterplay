using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    public GameObject projectilePrefabRingOnDeath;
    public GameObject projectilePrefabOnDeath;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponent<Faction>().IsFriendly(collision.GetComponentInParent<Faction>()) && !collision.isTrigger)
        {
            Destroy(collision);
            Destroy(gameObject);
            if (projectilePrefabOnDeath) {
                GetComponent<LaunchProjectile>().LaunchProjectileAtRandom(projectilePrefabOnDeath);
            }
            if (projectilePrefabRingOnDeath) {
                GetComponent<LaunchProjectile>().LaunchProjectileRing(projectilePrefabRingOnDeath, Random.Range(4, 9));
            }
        }
    }


}
