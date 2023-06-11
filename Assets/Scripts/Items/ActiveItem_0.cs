using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_0 : MonoBehaviour, IItem
{
    public void Activate()
    {
        GameObject.Find("GameplayManager").GetComponent<SpawnManager>().SpawnProjectileRing((GameObject)Resources.Load("Prefabs/Projectile"), transform.position, 10.0f, false, Color.white, 10);
    }
}
