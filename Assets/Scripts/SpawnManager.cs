using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    public GameObject map;
    private float offset = -2.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 0.1f;
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
        enemy.GetComponent<DirectlyMoveToward>().target = player;
        enemy.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
