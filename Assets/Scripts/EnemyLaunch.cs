using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaunch : MonoBehaviour
{
    private System.Func<Vector3, float, bool> isInMap;

    public System.Func<Vector3, float, bool> GetIsInMap()
    {
        return isInMap;
    }

    public void SetIsInMap(System.Func<Vector3, float, bool> value)
    {
        isInMap = value;
    }

    public void LaunchProjectile(GameObject projectilePrefab, Vector3 targetPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, targetPos - transform.position));
        projectile.GetComponent<SpriteRenderer>().color = Color.black;
        projectile.GetComponent<DestroyOutOfBounds>().SetIsInMap(GetIsInMap());
        projectile.GetComponent<Faction>().SetHostility(true);
    }

    public void LaunchProjectile(GameObject projectilePrefab)
    {
        float theta = Random.Range(-Mathf.PI, Mathf.PI);
        LaunchProjectile(projectilePrefab, transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
    }

    public void LaunchProjectileRing(GameObject projectilePrefab, int count)
    {
        for (int i = 0; i < count; i++) {
            float theta = 2 * Mathf.PI / count * i;
            LaunchProjectile(projectilePrefab, transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
        }
    }
}
