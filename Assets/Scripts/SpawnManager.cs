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

    void SpawnEnemy()
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
                enemy.GetComponent<DestroyOnHit>().projectilePrefabOnDeath = enemyProjectilePrefab;
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                enemy.GetComponent<DestroyOnHit>().projectilePrefabRingOnDeath = enemyProjectilePrefab;
                break;
        }
    }
}
