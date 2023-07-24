using UnityEngine;

public class PassiveItem_Weapon_2 : PassiveItem
{
    private static readonly string itemName = "Deadly Bomb";
    private static readonly string description = "Periodically leaves a bomb at current position.Bomb explodes after a delay and damages nearby enemies.";
    private static readonly string usage = "Passive: Weapon";
    private static readonly string logoPath = "Sprites/Items/Bomb";
    private float attackInterval = 2.0f;
    private float attackTimer = 0.0f;

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && GetComponent<IDisarmed>() == null)
        {
            Bomb.Instantiate(transform.position, Quaternion.identity, gameObject, GetComponent<Character>().GetHostility());
            attackTimer = attackInterval;
        }
    }

    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public static GameObject getShopOption()
    {
        GameObject shopOption = ShopOption.Instantiate();
        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(GetLogo());
        script.SetItemName(GetName());
        script.SetUsage(GetUsage());
        script.SetDescription(GetDescription());
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_Weapon_2>();
            UIManager.ClearShopPanel();
            GameplayManager.GetGoogleSender().SendMatrix4(GetName());
            GameplayManager.CloseShop();
        });
        return shopOption;
    }
}
