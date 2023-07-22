using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbsorber : MonoBehaviour, IDamageable
{
    private bool hostility;

    public bool GetHostility()
    {
        return hostility;
    }

    public void SetHostility(bool value)
    {
        hostility = value;
    }

    public void ReceiveDamage(Damage damage) { }
}
