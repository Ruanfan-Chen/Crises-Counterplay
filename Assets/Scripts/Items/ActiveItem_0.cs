using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ActiveItem_0 : ActiveItem
{
    private static readonly HashSet<ActiveItem_0> instances = new();
    private static readonly string itemName = "Supercharge";
    private static readonly string description = "Electricity zones now generate batteries which can charge this item. Releasing charged energy gains move speed bonus and deals damage to enemies colliding. Disarmed during activation.";
    private static readonly string usage = "Active: Press J";
    private static readonly string logoPath = "Sprites/Skills/Supercharge";
    private static readonly string notUsablePath = "Sprites/Skills/Supercharge";
    public static int activateCounter = 0;
    private int charge;
    private float displayedCharge;
    private readonly float duration = 5.0f;

    void Start()
    {
        ResetCharge();
    }

    void Update()
    {
        displayedCharge = Mathf.MoveTowards(displayedCharge, charge, Time.deltaTime * 2.0f);
    }

    public override void ResetCharge()
    {
        charge = 0;
        displayedCharge = 0.0f;
    }

    private void OnEnable()
    {
        instances.Add(this);
    }

    private void OnDisable()
    {
        instances.Remove(this);
    }

    public static bool GetBatterySpawn() { return instances.Count > 0; }

    public override void Activate()
    {
        if (IsUsable())
        {
            StartCoroutine(AddAndRemoveComponent<Buff>(gameObject, duration));
            charge--;
            activateCounter++;
        }
    }

    public override void Deactivate() { }

    public override float GetChargeProgress() => displayedCharge;

    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public override bool IsUsable() => charge > 0 && !GetComponent<Buff>();

    public override Sprite GetUISprite() => IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);

    public void Charge()
    {
        charge++;
    }

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
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_0>(KeyCode.J);
        });
        return shopOption;
    }

    public class Buff : MonoBehaviour, IInvulnerable, ISpeedBonus, IDisarmed
    {
        private float damage = 50.0f;
        private float speedBonus = 5.0f;
        private Color colorDifference;

        private void OnEnable()
        {
            colorDifference = Color.yellow - GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        private void OnDisable()
        {
            GetComponent<SpriteRenderer>().color -= colorDifference;
        }

        public float GetValue()
        {
            return speedBonus;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != GetComponent<Character>().GetHostility())
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                new Damage(gameObject, null, damageable, damage, direction).Apply();
            }
        }
    }
}
