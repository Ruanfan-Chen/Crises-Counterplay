using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_0 : ActiveItem
{
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Resources/Placeholder";
    private float maxCharge = 5.0f;
    private float charge = 5.0f;
    private float cost = 5.0f;

    void Update()
    {
        charge = Mathf.Clamp(charge + Time.deltaTime, 0.0f, maxCharge);
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override float GetChargeProgress()
    {
        return charge / maxCharge;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public override string GetName()
    {
        return itemName;
    }

    public override bool IsUsable()
    {
        return charge >= cost;
    }
}
