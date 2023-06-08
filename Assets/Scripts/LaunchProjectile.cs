using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject gameplayManager;



    public void Launch(GameObject projectilePrefab, Quaternion rotation)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
        projectile.GetComponent<DestroyOutOfBounds>().gameplayManager = gameplayManager;
        projectile.GetComponent<Faction>().SetHostility(GetComponent<Faction>().GetHostility());
        if (GetComponent<Faction>().GetHostility())
            projectile.GetComponent<SpriteRenderer>().color = Color.black;
        else
            projectile.GetComponent<SpriteRenderer>().color = Color.white;
        projectile.tag = "Disposable";
    }
    public void Launch(GameObject projectilePrefab, Vector3 targetPos)
    {
        Launch(projectilePrefab, Quaternion.LookRotation(Vector3.forward, targetPos - transform.position));
    }

    public void LaunchProjectileAtRandom(GameObject projectilePrefab)
    {
        float theta = Random.Range(-Mathf.PI, Mathf.PI);
        Launch(projectilePrefab, transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
    }

    public void LaunchProjectileRing(GameObject projectilePrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float theta = 2 * Mathf.PI / count * i;
            Launch(projectilePrefab, transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
        }
    }
}
