using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltTimer : MonoBehaviour
{
    private static readonly HashSet<HaltTimer> instances = new();
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
