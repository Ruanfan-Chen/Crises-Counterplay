using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_0 : MonoBehaviour, IItem
{
    public void Activate()
    {
        Character character = GetComponent<Character>();
        List<GameObject> ring = Projectile.InstantiateRing(transform.position, 0, 10);
        //modify
    }
}
