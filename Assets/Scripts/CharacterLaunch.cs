using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunch : MonoBehaviour
{
    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (attackTimer <= 0 && collision.GetComponent<EnemyOnHit>() && !GetComponentInParent<Faction>().IsFriendly(collision.GetComponentInParent<Faction>()))
        {
        attackTimer = GetComponentInParent<Character>().GetAttackInterval();
        }*/
    }
}