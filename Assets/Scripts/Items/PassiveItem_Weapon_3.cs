using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

public class PassiveItem_Weapon_3 : PassiveItem
{
    private static readonly string itemName = "Ancient Boomerang";
    private static readonly string description = "Spawns a boomerang that loosely follows behind and can be swung like a flail to damage enemies.";
    private static readonly string usage = "Passive: Weapon";
    private static readonly string logoPath = "Sprites/Items/Boomerang";
    private static readonly float g = 5.0f;
    private static readonly float damp = 0.2f;
    private GameObject boomerang;
    private Vector3 boomerangVelocity = Vector3.zero;
    void Update()
    {
        if (GetComponent<IDisarmed>() != null)
        {
            boomerang.transform.position = transform.position;
        }
        else
        {
            Vector3 relativPos = boomerang.transform.position - transform.position;
            boomerangVelocity -= Time.deltaTime * (g * relativPos + damp * boomerangVelocity);
            boomerang.transform.position += boomerangVelocity * Time.deltaTime;
        }
    }
    private void OnEnable()
    {
        boomerang = Boomerang.Instantiate(transform.position, Quaternion.identity, gameObject, GetComponent<Character>().GetHostility());
    }

    private void OnDisable()
    {
        Destroy(boomerang);
    }
    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public static GameObject GetShopOption()
    {
        GameObject shopOption = ShopOption.Instantiate();
        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(GetLogo());
        script.SetItemName(GetName());
        script.SetUsage(GetUsage());
        script.SetDescription(GetDescription());
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_Weapon_3>();
        });
        return shopOption;
    }
}
