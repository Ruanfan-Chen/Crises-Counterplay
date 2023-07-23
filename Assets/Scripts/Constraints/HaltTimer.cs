using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltTimer : MonoBehaviour
{
    private static readonly HashSet<HaltTimer> instances = new();
    private static readonly string spritePath = "Sprites/sandClock";
    private GameObject logo;
    private void OnEnable()
    {
        instances.Add(this);
        logo = new("HaltTimerLogo", new[] { typeof(SpriteRenderer) });
        logo.transform.SetParent(transform);
        logo.transform.localPosition = new(1.0f, -0.75f, 0.0f);
        logo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
    }

    private void OnDisable()
    {
        instances.Remove(this);
        Destroy(logo);
    }

    public static bool ExistInstance()
    {
        return instances.Count > 0;
    }
}
