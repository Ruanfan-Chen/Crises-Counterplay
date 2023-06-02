using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    private float startDelay = 2.0f;
    private float spawnInterval = 0.1f;
    private float xMin = -30.0f;
    private float xMax = 30.0f;
    private float yMin = -30.0f;
    private float yMax = 30.0f;
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
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0), new Quaternion());
        enemy.GetComponent<DirectlyMoveToward>().target = player;
        enemy.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
