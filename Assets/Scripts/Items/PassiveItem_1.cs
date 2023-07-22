using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_1 : PassiveItem
{
    private static readonly string prefabPath = "Prefabs/BlackHole";
    private static readonly string itemName = "BlackHole";
    private static readonly string description = "Description Placeholder";
    private static readonly string logoPath = "Sprites/Skills/Placeholder";
    private static readonly float radius = 1.25f;
    private static readonly float angularVelocity = 15.0f;
    private float angularDisplacement;

    private GameObject[] blackHoles = new GameObject[3];

    private void Update()
    {
        angularDisplacement += Time.deltaTime * angularVelocity;
        for (int i = 0; i < 3; i++)
        {
            blackHoles[i].transform.localPosition = Quaternion.Euler(0.0f, 0.0f, 120.0f * i + angularDisplacement) * Vector3.up * radius;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < 3; i++)
        {
            blackHoles[i] = Instantiate(Resources.Load<GameObject>(prefabPath), transform);
        }
        angularDisplacement = 0.0f;
    }

    private void OnDisable()
    {
        for (int i = 0; i < 3; i++)
            if (blackHoles[i] != null)
                Destroy(blackHoles[i]);
    }

    public static string GetDescription()
    {
        return description;
    }

    public static Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public static string GetName()
    {
        return itemName;
    }
}
