using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnHit : MonoBehaviour
{
    public GameObject projectilePrefabOnDeath;
    public GameObject projectilePrefabRingOnDeath;
    private float projectileSpeed = 5.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && !GetComponent<Faction>().IsFriendly(collision.GetComponent<Faction>()))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            if (projectilePrefabOnDeath)
            {
                /*
                 Launch Projectile here
                 */
            }
            if (projectilePrefabRingOnDeath)
            {
                /*
                 Launch Projectile here
                 */
            }
        }
    }
}
