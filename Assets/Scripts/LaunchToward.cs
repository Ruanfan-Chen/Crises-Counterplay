using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchToward : MonoBehaviour
{
    public GameObject target;
    public GameObject projectilePrefab;
    public GameObject map;
    private float startDelay = 2.0f;
    private float attackInterval = 2.0f;
    private bool hostility = true;
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
        float theta = Random.Range(-Mathf.PI, Mathf.PI);
        Vector3 displacement = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
        if (target)
            displacement = target.transform.position - transform.position;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, displacement));
        projectile.GetComponent<SpriteRenderer>().color = Color.red;
        projectile.GetComponent<DestroyOutOfBounds>().map = map;
        projectile.GetComponent<Faction>().SetHostility(hostility);
    }
}
