using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyProjectilePrefab;
    public GameObject player;
    public GameObject map;
    private float offset = -2.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 0.3f;
    private Vector3 min;
    private Vector3 max;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnEnemy", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnEnemy()
    {
        min = map.transform.position - map.transform.lossyScale / 2;
        max = map.transform.position + map.transform.lossyScale / 2;
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(min.x - offset, max.x + offset), Random.Range(min.y - offset, max.y + offset), 0), new Quaternion());
        enemy.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        enemy.GetComponent<Faction>().SetHostility(true);
        enemy.GetComponent<EnemyLaunch>().map = map;
        switch (Random.Range(0, 2))
        {
            case 0:
                enemy.AddComponent<AimlesslyMove>();
                break;
            case 1:
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
        switch (Random.Range(0, 4))
        {
            case 0:
                break;
            case 1:
                enemy.GetComponent<DestroyOnHit>().projectilePrefabOnDeath = enemyProjectilePrefab;
                enemy.GetComponent<DestroyOnHit>().projectilePrefabRingOnDeath = null;
                break;
            case 2:
                enemy.GetComponent<DestroyOnHit>().projectilePrefabOnDeath = null;
                enemy.GetComponent<DestroyOnHit>().projectilePrefabRingOnDeath = enemyProjectilePrefab;
                break;
            case 3:
                enemy.GetComponent<DestroyOnHit>().projectilePrefabOnDeath = enemyProjectilePrefab;
                enemy.GetComponent<DestroyOnHit>().projectilePrefabRingOnDeath = enemyProjectilePrefab;
                break;
        }
    }
}
