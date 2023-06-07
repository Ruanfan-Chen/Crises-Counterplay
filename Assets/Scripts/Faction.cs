using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
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

    public bool IsFriendly(Faction faction)
    {
        return hostility == faction.GetHostility();
    }
}
