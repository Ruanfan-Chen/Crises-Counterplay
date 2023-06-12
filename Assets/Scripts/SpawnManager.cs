using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyProjectilePrefab;
    public GameObject player;
    private float offset = 2.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", startDelay, spawnInterval);
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, Quaternion rotation, Color color)
    {
        GameObject enemy = Instantiate(prefab, position, rotation);
        enemy.GetComponent<SpriteRenderer>().color = color;
        enemy.tag = "Disposable";
        return enemy;
    }

    public GameObject SpawnEnemy()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-120, 120), Random.Range(-120, 120), 0);
        } while (!GetComponent<MapManager>().IsInMap(position, offset) || (position-player.transform.position).magnitude <= offset);
        GameObject enemy = SpawnEnemy(enemyPrefab, position, new Quaternion(), Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f));
        switch (Random.Range(0, 3))
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<AimlesslyMove>();
                break;
            case 2:
                enemy.AddComponent<DirectlyMoveToward>();
                enemy.GetComponent<DirectlyMoveToward>().target = player;
                break;
        }

        switch (Random.Range(0, 3))
        {
            case 0:
                break;
            case 1:
                //enemy.AddComponent<LaunchToward>();
                //enemy.GetComponent<LaunchToward>().targetGameObj = player;
                //enemy.GetComponent<LaunchToward>().projectilePrefab = enemyProjectilePrefab;
                break;
            case 2:
                //enemy.AddComponent<LaunchToward>();
                //enemy.GetComponent<LaunchToward>().targetGameObj = null;
                //enemy.GetComponent<LaunchToward>().projectilePrefab = enemyProjectilePrefab;
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                //enemy.GetComponent<EnemyOnHit>().projectilePrefabOnDeath = enemyProjectilePrefab;
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                //enemy.GetComponent<EnemyOnHit>().projectilePrefabRingOnDeath = enemyProjectilePrefab;
                break;
        }
        return enemy;
    }

    public GameObject SpawnProjectile(GameObject prefab, Vector3 position, Quaternion rotation, float speed, bool hostility)
    {
        GameObject projectile = Instantiate(prefab, position, rotation);
        projectile.GetComponent<Projectile>().SetSpeed(speed);
        projectile.GetComponent<Projectile>().SetHostility(hostility);
        return projectile;
    }
    public GameObject SpawnProjectile(GameObject prefab, Vector3 position, Vector3 lookAt, float speed, bool hostility)
    {
        return SpawnProjectile(prefab, position, Quaternion.LookRotation(Vector3.forward, lookAt - position), speed, hostility);
    }

    public GameObject SpawnProjectile(GameObject prefab, Vector3 position, float theta, float speed, bool hostility)
    {
        return SpawnProjectile(prefab, position, position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta)), speed, hostility);
    }

    public List<GameObject> SpawnProjectileRing(GameObject prefab, Vector3 position, float speed, bool hostility, int count)
    {
        List<GameObject> projectileList = new();
        float thetaBase = Random.Range(-Mathf.PI, Mathf.PI);
        for (int i = 0; i < count; i++)
        {
            float theta = thetaBase + 2 * Mathf.PI / count * i;
            projectileList.Add(SpawnProjectile(prefab, position, theta, speed, hostility));
        }
        return projectileList;
    }
}
