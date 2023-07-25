using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_3 : PassiveItem
{
    private static readonly string itemName = "Jade Figurine";
    private static readonly string description = "When not moving, the character recovers 20% HP per second.";
    private static readonly string usage = "Passive";
    private static readonly string logoPath = "Sprites/Items/Jade Figurine";
    private readonly float threshold = 1.0f;
    private readonly int heal = 5;
    private float timer = 0.0f;
    private Vector3 lastFramePos = Vector3.zero;

    void Update()
    {
        if (lastFramePos == transform.position)
            timer += Time.deltaTime;
        else
        {
            lastFramePos = transform.position;
            timer = 0.0f;
        }
        if (timer >= threshold)
        {
            timer -= threshold;
            GameplayManager.getCharacter().GetComponent<Character>().Heal(heal);
        }
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
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_3>();
        });
        return shopOption;
    }

}
