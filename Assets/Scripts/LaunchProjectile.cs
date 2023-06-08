using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject gameplayManager;



    public void Launch(GameObject projectilePrefab, float projectileSpeed, Quaternion rotation)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
        projectile.GetComponent<DestroyOutOfBounds>().gameplayManager = gameplayManager;
        projectile.GetComponent<Faction>().SetHostility(GetComponent<Faction>().GetHostility());
        projectile.GetComponent<ProjectileMove>().SetSpeed(projectileSpeed);
        if (GetComponent<Faction>().GetHostility())
            projectile.GetComponent<SpriteRenderer>().color = Color.black;
        else
            projectile.GetComponent<SpriteRenderer>().color = Color.white;
        projectile.tag = "Disposable";
    }
    public void Launch(GameObject projectilePrefab, float projectileSpeed, Vector3 targetPos)
    {
        Launch(projectilePrefab, projectileSpeed, Quaternion.LookRotation(Vector3.forward, targetPos - transform.position));
    }

    public void LaunchProjectileAtRandom(GameObject projectilePrefab, float projectileSpeed)
    {
        float theta = Random.Range(-Mathf.PI, Mathf.PI);
        Launch(projectilePrefab, projectileSpeed, transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
    }

    public void LaunchProjectileRing(GameObject projectilePrefab, float projectileSpeed, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float theta = 2 * Mathf.PI / count * i;
            Launch(projectilePrefab, projectileSpeed, transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
        }
    }
}
