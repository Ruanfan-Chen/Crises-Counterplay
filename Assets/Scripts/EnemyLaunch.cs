using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaunch : MonoBehaviour
{
    public GameObject gameplayManager;

    public void LaunchProjectile(GameObject projectilePrefab, Vector3 targetPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, targetPos - transform.position));
        
        //to organize enemies and projectiles
        GameObject disposable = GameObject.Find("disposable");
        if (!disposable)
        {
            disposable = new GameObject("disposable");
        }
        projectile.transform.SetParent(disposable.transform);

        projectile.GetComponent<SpriteRenderer>().color = Color.black;
        projectile.GetComponent<DestroyOutOfBounds>().gameplayManager = gameplayManager;
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
