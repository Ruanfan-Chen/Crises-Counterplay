using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_1 : ActiveItem
{
    private static string itemName = "Supercharge";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Ebbtide";
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        throw new System.NotImplementedException();
    }

    public override Sprite GetUISprite()
    {
        return GetLogo();
    }

    public override bool IsUsable()
    {
        throw new System.NotImplementedException();
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
