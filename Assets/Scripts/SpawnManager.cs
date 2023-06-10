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

    // Update is called once per frame
    void Update()
    {

    }

    GameObject SpawnEnemy()
    {
        Vector3 position = new Vector3(Random.Range(-120, 120), Random.Range(-120, 120), 0);
        while (GetComponent<MapManager>().PosInMap(position, offset) != position)
        {
            position = new Vector3(Random.Range(-120, 120), Random.Range(-120, 120), 0);
        }
        GameObject enemy = Instantiate(enemyPrefab, position, new Quaternion());

        //to organize enemies and projectiles
        GameObject disposable = GameObject.Find("disposable");
        if(!disposable)
        {
            disposable = new GameObject("disposable");
        }
        enemy.transform.SetParent(disposable.transform);



        enemy.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        enemy.GetComponent<Faction>().SetHostility(true);
        enemy.GetComponent<LaunchProjectile>().gameplayManager = gameObject;
        enemy.tag = "Disposable";
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
                enemy.AddComponent<LaunchToward>();
                enemy.GetComponent<LaunchToward>().targetGameObj = player;
                enemy.GetComponent<LaunchToward>().projectilePrefab = enemyProjectilePrefab;
                break;
            case 2:
                enemy.AddComponent<LaunchToward>();
                enemy.GetComponent<LaunchToward>().targetGameObj = null;
                enemy.GetComponent<LaunchToward>().projectilePrefab = enemyProjectilePrefab;
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                enemy.GetComponent<EnemyOnHit>().projectilePrefabOnDeath = enemyProjectilePrefab;
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                enemy.GetComponent<EnemyOnHit>().projectilePrefabRingOnDeath = enemyProjectilePrefab;
                break;
        }
        return enemy;
    }

    public GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation, float speed, bool hostility, Color color)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, rotation);
        projectile.GetComponent<DestroyOutOfBounds>().gameplayManager = gameObject;
        projectile.GetComponent<ProjectileMove>().SetSpeed(speed);
        projectile.GetComponent<Faction>().SetHostility(hostility);
        projectile.GetComponent<SpriteRenderer>().color = color;
        projectile.tag = "Disposable";
        return projectile;
    }
    public GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position, Vector3 lookAt, float speed, bool hostility, Color color)
    {
        return SpawnProjectile(projectilePrefab, position, Quaternion.LookRotation(Vector3.forward, lookAt - position), speed,hostility,color);
    }

    public GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position, float theta, float speed, bool hostility, Color color)
    {
        return SpawnProjectile(projectilePrefab, position, position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta)), speed, hostility, color);
    }

    public List<GameObject> SpawnProjectileRing(GameObject projectilePrefab, Vector3 position, float speed, bool hostility, Color color, int count)
    {
        List<GameObject> projectileList = new();
        float thetaBase = Random.Range(-Mathf.PI, Mathf.PI);
        for (int i = 0; i < count; i++)
        {
            float theta = thetaBase + 2 * Mathf.PI / count * i;
            projectileList.Add(SpawnProjectile(projectilePrefab, position, theta, speed, hostility, color));
        }
        return projectileList;
    }
}
