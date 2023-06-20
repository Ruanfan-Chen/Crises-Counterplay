using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_0 : ActiveItem
{
    private float angleOfView = 120.0f;
    private int count = 6;
    public override void Activate()
    {
        Character character = GetComponent<Character>();
        for (float i = 0; i < count; i++)
            Projectile.Instantiate(transform.position, Quaternion.Euler(0, 0, (i / (count - 1) - 0.5f) * angleOfView) * transform.rotation, GetComponents<IProjectileModifier>());
    }
}
