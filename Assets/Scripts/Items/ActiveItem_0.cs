using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_0 : ActiveItem
{
    private int count = 6;
    public override void Activate()
    {
        Character character = GetComponent<Character>();
        float angleOfView=character.GetAngleOfView();
        for (float i = 0; i < count; i++)
            Projectile.Instantiate(transform.position, Quaternion.Euler(0, 0, (i / (count - 1) - 0.5f) * angleOfView) * transform.rotation, GetComponents<IProjectileModifier>());
    }
}
