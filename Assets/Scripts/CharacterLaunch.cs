using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject gameplayManager;
    private float startDelay = 2.0f;
    private float attackInterval = 0.2f;
    private float angleOfLaunch = 90.0f;
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-angleOfLaunch / 2, angleOfLaunch / 2)));
        projectile.GetComponent<SpriteRenderer>().color = Color.white;
        projectile.GetComponent<DestroyOutOfBounds>().gameplayManager = gameplayManager;
        projectile.GetComponent<Faction>().SetHostility(false);
    }
}
