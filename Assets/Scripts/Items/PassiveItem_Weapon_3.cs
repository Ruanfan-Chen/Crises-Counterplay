using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

public class PassiveItem_Weapon_3 : PassiveItem
{
    private static readonly string itemName = "Ancient Boomerang";
    private static readonly string description = "The character throws the boomerang to a nearby enemy. Once upon hit, the boomerang will return. The character is unable to attack until the boomerang returns.";
    private static readonly string usage = "Passive: Weapon";
    private static readonly string logoPath = "Sprites/Items/Boomerang";
    private GameObject view;
    private GameObject boomerang;
    private float range = 10.0f;
    private float speed = 7.5f;
    private Transform target;

    void Start()
    {
        target = transform;
    }
    void Update()
    {
        if (GetComponent<IDisarmed>() != null)
            boomerang.transform.localPosition = Vector3.zero;
        else
        {
            if (target == null || (boomerang.transform.position - transform.position).magnitude < speed * Time.deltaTime || (boomerang.transform.position - transform.position).magnitude > range)
            {
                if (target == transform)
                {
                    IEnumerable<GameObject> targetables = OverlapGameObject(view, collider => (collider.GetComponent<IDamageable>() != null) && (collider.GetComponent<IDamageable>().GetHostility() != GetComponent<Character>().GetHostility()));
                    if (targetables.Count() != 0)
                        target = targetables.ElementAt(Random.Range(0, targetables.Count())).transform;
                }
                else
                    target = transform;
            }
            boomerang.transform.position = Vector3.MoveTowards(boomerang.transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        view = new GameObject(itemName + "WeaponView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        CircleCollider2D viewTrigger = view.AddComponent<CircleCollider2D>();
        viewTrigger.isTrigger = true;
        viewTrigger.radius = range;
        view.AddComponent<Rigidbody2D>().isKinematic = true;

        boomerang = Boomerang.Instantiate(transform.position, Quaternion.identity, gameObject, GetComponent<Character>().GetHostility());
    }

    private void OnDisable()
    {
        Destroy(view);
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
