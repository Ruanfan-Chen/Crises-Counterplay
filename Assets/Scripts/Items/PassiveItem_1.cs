using UnityEngine;

public class PassiveItem_1 : PassiveItem
{
    private static readonly string prefabPath = "Prefabs/BlackHole";
    private static readonly string itemName = "BlackHole";
    private static readonly string description = "Summon three blackholes around the character. Blackholes will block projectiles.";
    private static readonly string usage = "Passive";
    private static readonly string logoPath = "Sprites/Items/Blackhole";
    private static readonly float radius = 1.45f;
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
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_1>();
        });
        return shopOption;
    }
}
