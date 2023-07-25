using System.Collections.Generic;
using UnityEngine;

public class ForcedBatterySpawn : MonoBehaviour
{
    private static readonly HashSet<ForcedBatterySpawn> instances = new();
    private void OnEnable()
    {
        instances.Add(this);
    }

    private void OnDisable()
    {
        instances.Remove(this);
    }

    public static bool ExistInstance()
    {
        return instances.Count > 0;
    }
}
