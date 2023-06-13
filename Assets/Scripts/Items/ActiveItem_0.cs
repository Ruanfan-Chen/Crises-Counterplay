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
        List<GameObject> projectiles = new();
        for (float i = 0; i < count; i++)
            projectiles.Add(Projectile.Instantiate(transform.position, Quaternion.Euler(0, 0, (i / (count - 1) - 0.5f) * angleOfView) * transform.rotation));
        foreach (GameObject projectile in projectiles)
            foreach (IProjectileModifier modifier in GetComponents<IProjectileModifier>())
                modifier.Modify(projectile);
    }
}
