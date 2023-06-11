using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnHit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (!collision.isTrigger && !GetComponent<Faction>().IsFriendly(collision.GetComponent<Faction>()))
        {
            if (collision.GetComponent<ProjectileMove>())
            {

                GetComponent<Character>().ReceiveDmg(25.0f);
                Destroy(collision.gameObject);
            }
            else
                GetComponent<Character>().ReceiveDmg(25.0f * Time.deltaTime);
        }*/
    }
}