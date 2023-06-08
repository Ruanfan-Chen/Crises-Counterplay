using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnHit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger && !GetComponent<Faction>().IsFriendly(collision.GetComponent<Faction>()))
        {
            GetComponent<CharacterAttribute>().TakeDmg(25.0f * Time.deltaTime);
            if (collision.GetComponent<ProjectileMove>())
                Destroy(collision.gameObject);
        }
    }
}
