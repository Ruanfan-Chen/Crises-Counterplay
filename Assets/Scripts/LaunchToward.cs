using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchToward : MonoBehaviour
{
    public GameObject targetGameObj;
    public GameObject projectilePrefab;
    private float startDelay = 2.0f;
    private float attackInterval = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("LaunchProjectile", startDelay, attackInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LaunchProjectile()
    {
        if (targetGameObj)
            GetComponent<LaunchProjectile>().Launch(projectilePrefab, targetGameObj.transform.position);
        else
            GetComponent<LaunchProjectile>().LaunchProjectileAtRandom(projectilePrefab);
    }
}
