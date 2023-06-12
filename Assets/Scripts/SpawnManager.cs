using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject player;
    private float offset = 2.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay, spawnInterval);
    }

    public GameObject SpawnRandomEnemy()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-120, 120), Random.Range(-120, 120), 0);
        } while (!GetComponent<MapManager>().IsInMap(position, offset) || (position-player.transform.position).magnitude <= offset);
        GameObject enemy = Enemy.Instantiate(position, new Quaternion());
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
}
