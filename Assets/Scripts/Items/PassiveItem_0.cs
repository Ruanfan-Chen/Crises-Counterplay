using UnityEngine;
public class PassiveItem_0 : PassiveItem
{
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Toxic Footprint";
    private float contactDPS = 25.0f;
    // maximun duration (if do 0 damage ro enemies)
    private float maxDuration = 10.0f;
    // maximun total Damage to enemies
    private float maxDamage = 250.0f;

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
}