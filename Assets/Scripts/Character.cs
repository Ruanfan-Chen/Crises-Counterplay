using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class Character : MonoBehaviour, IProjectileModifier, IDamageable
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    private static float moveSpeed = 5.0f;
    private List<PassiveItem> passiveItems = new();
    private BiDictionary<KeyCode, ActiveItem> activeItems = new();
    public static readonly float invDurationOnDmg = 0.7f;
    public static readonly float knockbackDurationOnDmg = 0.5f;
    public static readonly float knockbackDistanceOnDmg = 3.0f;
    public static readonly float initialKnockbackSpeedOnDmg = 50.0f;

    public float GetHealth() { return health; }

    public void SetHealth(float value) { health = value; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetMaxHealth(float value) { maxHealth = value; }

    public static float GetMoveSpeed() { return moveSpeed; }

    public static void SetMoveSpeed(float value) { moveSpeed = value; }

    public void ReceiveDamage(Damage damage)
    {
        if (GetComponent<IInvulnerable>() != null) return;
        health -= damage.GetValue();
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        IEnumerator coroutine = damage.GetCoroutine();
        if (coroutine == null)
        {
            Vector3 sourcePos = transform.position;
            if (damage.GetSource())
                sourcePos = damage.GetSource().transform.position;
            if (damage.GetMedium())
                sourcePos = damage.GetMedium().transform.position;
            coroutine = ForcedMovement(transform, (transform.position - sourcePos).normalized * knockbackDistanceOnDmg, initialKnockbackSpeedOnDmg, knockbackDurationOnDmg);
        }
        StartCoroutine(coroutine);
        StartCoroutine(AddAndRemoveComponent<Invulnerable>(gameObject, invDurationOnDmg));
    }
    public IReadOnlyList<PassiveItem> GetPassiveItems() { return passiveItems; }
    public IReadOnlyDictionary<KeyCode, ActiveItem> GetKeyCodeActiveItemPairs() { return activeItems.GetTUDict(); }
    public IReadOnlyDictionary<ActiveItem, KeyCode> GetActiveItemKeyCodePairs() { return activeItems.GetUTDict(); }
    public ActiveItem GetActiveItem(KeyCode keyCode)
    {
        return activeItems.TryGetValue(keyCode, out ActiveItem item) ? item : null;
    }
    public KeyCode GetKeyCode(ActiveItem item)
    {
        return activeItems.TryGetValue(item, out KeyCode keyCode) ? keyCode : KeyCode.None;
    }
    public bool RemoveItem(PassiveItem item)
    {
        if (passiveItems.Remove(item))
        {
            Destroy(item);
            return true;
        }
        return false;
    }
    public bool RemoveItem(KeyCode keyCode)
    {
        ActiveItem item = GetActiveItem(keyCode);
        return item ? RemoveItem(item) : false;
    }
    public bool RemoveItem(ActiveItem item)
    {
        if (activeItems.Remove(item))
        {
            Destroy(item);
            return true;
        }
        return false;
    }
    public PassiveItem GiveItem<T>() where T : PassiveItem
    {
        PassiveItem item = gameObject.AddComponent<T>();
        passiveItems.Add(item);
        return item;
    }

    public ActiveItem GiveItem<T>(KeyCode keyCode) where T : ActiveItem
    {
        RemoveItem(keyCode);
        ActiveItem item = gameObject.AddComponent<T>();
        activeItems.Add(keyCode, item);
        return item;
    }
    public void ActivateItem(KeyCode keyCode)
    {
        ActiveItem item = GetActiveItem(keyCode);
        if (item)
            item.Activate();
    }
    public bool GetHostility() { return false; }

    void IProjectileModifier.Modify(GameObject projectile)
    {
        Projectile script = projectile.GetComponent<Projectile>();
        script.SetHostility(false);
        script.SetColor(GetComponent<SpriteRenderer>().color);
        script.SetSource(gameObject);
    }

    void Start()
    {
        GiveItem<PassiveItem_Weapon_0>();
    }

    void Update()
    {
        foreach (KeyValuePair<KeyCode, ActiveItem> keyValuePair in activeItems.GetTUDict())
        {
            if (Input.GetKeyDown(keyValuePair.Key))
                keyValuePair.Value.Activate();
            if (Input.GetKeyUp(keyValuePair.Key))
                keyValuePair.Value.Deactivate();
        }
    }
}
