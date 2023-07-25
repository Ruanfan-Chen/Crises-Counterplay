using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ActiveItem_0 : ActiveItem
{
    private static readonly HashSet<ActiveItem_0> instances = new();
    private static readonly string itemName = "Supercharge";
    private static readonly string description = "Electricity zones now have 25% chance generating batteries which can charge this ability. Releasing charged energy gains move speed bonus and deals damage to enemies colliding. Disarmed during activation.";
    private static readonly string usage = "Active: Press J";
    private static readonly string logoPath = "Sprites/Skills/Supercharge";
    private static readonly string notUsablePath = "Sprites/Skills/Supercharge";
    public static int activateCounter = 0;
    private readonly float duration = 2.2f;
    private int charge;
    private readonly HashSet<ChargeBuffer> buffers = new();

    void Start()
    {
        ResetCharge();
    }

    void Update()
    {
        foreach (ChargeBuffer buffer in buffers)
            buffer.Update();
        buffers.RemoveWhere(buffer => buffer.GetValue() == 0.0f);
    }
    public override void ResetCharge()
    {
        charge = 0;
    }

    private void OnEnable()
    {
        instances.Add(this);
    }

    private void OnDisable()
    {
        instances.Remove(this);
    }

    public static bool GetBatterySpawn() => instances.Count != 0 && (ForcedBatterySpawn.ExistInstance() || WeightedRandom(new Dictionary<bool, float>() { [true] = 0.25f, [false] = 0.75f }));

    public override void Activate()
    {
        if (IsUsable())
        {
            StartCoroutine(AddAndRemoveComponent<Buff>(gameObject, duration));
            charge--;
            buffers.Add(new(1.0f, duration));
            activateCounter++;
        }
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        float sum = charge;
        foreach (ChargeBuffer buffer in buffers)
            sum += buffer.GetValue();
        return sum;
    }

    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public override bool IsUsable() => charge > 0 && !GetComponent<Buff>();

    public override Sprite GetUISprite() => IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);

    public void Charge()
    {
        charge++;
        buffers.Add(new(-1.0f, 0.5f));
    }

    private class ChargeBuffer
    {
        private float value;
        private readonly float changeRate;

        public ChargeBuffer(float value, float duration)
        {
            this.value = value;
            changeRate = Mathf.Abs(value) / duration;
        }
        public void Update() => value = Mathf.MoveTowards(value, 0.0f, changeRate * Time.deltaTime);

        public float GetValue() => value;
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

    public class Buff : MonoBehaviour, IInvulnerable, ISpeedBonus, IDisarmed, IDisposable
    {
        private float damage = 50.0f;
        private float speedBonus = 2.5f;
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
