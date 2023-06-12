using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour
{
}

public abstract class ActiveItem : MonoBehaviour
{
    public abstract void Activate();
}

public interface IProjectileModifier
{
    public void Modify(GameObject projectile);
}