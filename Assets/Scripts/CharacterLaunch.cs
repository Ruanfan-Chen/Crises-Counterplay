using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject map;
    private float startDelay = 2.0f;
    private float attackInterval = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("launchProjectile", startDelay, attackInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void launchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-45.0f, 45.0f)));
        projectile.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        projectile.GetComponent<DestroyOutOfBounds>().map = map;
    }
}
