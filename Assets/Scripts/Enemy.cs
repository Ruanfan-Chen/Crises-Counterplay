using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Enemy";
    private float contactDPS = 25.00f;

    public float GetContactDPS() { return contactDPS; }

    public void SetContactDPS(float value) { contactDPS = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Disposable";
        GetComponent<ConstraintInsideOfMap>().SetOffset(0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (!collision.isTrigger && !GetComponent<Faction>().IsFriendly(collision.GetComponent<Faction>()))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            if (projectilePrefabOnDeath)
            {
                
                 Launch Projectile here
                 
            }
            if (projectilePrefabRingOnDeath)
            {
                
                 Launch Projectile here
                 
            }
        }*/
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        return Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
    }
}
