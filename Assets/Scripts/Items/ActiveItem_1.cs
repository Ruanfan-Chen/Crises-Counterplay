using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveItem_1 : ActiveItem
{
    private static string itemName = "EbbTide";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Ebbtide";
    private static string tutorialPath = "Sprites/Tutorial/Tutorial_EbbTide";
    private static string notUsablePath = "Sprites/Skills/Ebbtide";

    private float speed = 10.0f;
    public override void Activate()
    {
        if (IsUsable())
        {
            List<Vector3> positions = GetComponent<Waterblight>().GetTrail();
            StartCoroutine(Back(positions));
        }
    }

    private IEnumerator Back(List<Vector3> positions)
    {
        ActiveItem_0.Buff buff = gameObject.AddComponent<ActiveItem_0.Buff>();
        for (int i = positions.Count - 1; i > 0; i--)
        {
            Vector3 displacement = positions[i - 1] - positions[i];
            transform.position = positions[i];
            yield return new WaitForSeconds(displacement.magnitude / speed);
        }
        Destroy(buff);
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        return 1.0f;
    }

    public override Sprite GetUISprite()
    {
        return IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);
    }

    public override bool IsUsable()
    {
        return GetComponent<Waterblight>() != null;
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

    public static Sprite GetTutorial()
    {
        return Resources.Load<Sprite>(tutorialPath);
    }
}
