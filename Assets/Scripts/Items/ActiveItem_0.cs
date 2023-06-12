using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_0 : ActiveItem
{
    public override void Activate()
    {
        Character character = GetComponent<Character>();
        List<GameObject> ring = Projectile.InstantiateRing(transform.position, 0, 10);
        //modify
    }
}
