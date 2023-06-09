using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnHit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger && !GetComponent<Faction>().IsFriendly(collision.GetComponent<Faction>()))
        {
            if (collision.GetComponent<ProjectileMove>())
            {

                GetComponent<CharacterAttribute>().TakeDmg(25.0f);
                Destroy(collision.gameObject);
            }
            else
                GetComponent<CharacterAttribute>().TakeDmg(25.0f * Time.deltaTime);
        }
    }
}