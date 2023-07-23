using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class Character : MonoBehaviour, IProjectileModifier, IDamageable
{
    public static readonly string projectilePrefabPath = "Sprites/playerBullet";
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private Bar healthBar;
    private float moveSpeed = 5.0f;
    private List<PassiveItem> passiveItems = new();
    private BiDictionary<KeyCode, ActiveItem> activeItems = new();
    public static readonly float invDurationOnDmg = 0.7f;
    public static readonly float knockbackDurationOnDmg = 0.5f;
    public static readonly float knockbackDistanceOnDmg = 3.0f;
    public static readonly float initialKnockbackSpeedOnDmg = 50.0f;
    private Vector3 velocity = Vector3.zero;

    public float GetHealth() { return health; }

    public void SetHealth(float value) { health = value; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetMaxHealth(float value) { maxHealth = value; }

    public float GetMoveSpeed()
    {
        float bonus = 0.0f;
        foreach (ISpeedBonus buff in GetComponents<ISpeedBonus>())
            bonus += buff.GetValue();
        return moveSpeed + bonus;
    }

    public void SetMoveSpeed(float value) { moveSpeed = value; }

    public void Heal(int value) { health = Mathf.Clamp(health + value, 0.0f, maxHealth); }

    public void ReceiveDamage(Damage damage)
    {
        if (GetComponent<IInvulnerable>() != null) return;
        health -= damage.GetValue();
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        StartCoroutine(ForcedMovement(transform, damage.GetDiretcion() * knockbackDistanceOnDmg, initialKnockbackSpeedOnDmg, knockbackDurationOnDmg));
        StartCoroutine(AddAndRemoveComponent<Invulnerable>(gameObject, invDurationOnDmg));
        if (damage.GetSource())
            GameplayManager.GetGoogleSender().SendMatrix1(LevelManager.GetLevelName(), damage.GetSource().name);
        else if (damage.GetMedium())
            GameplayManager.GetGoogleSender().SendMatrix1(LevelManager.GetLevelName(), damage.GetMedium().name);
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
    public T GiveItem<T>() where T : PassiveItem
    {
        T item = gameObject.AddComponent<T>();
        passiveItems.Add(item);
        return item;
    }

    public T GiveItem<T>(KeyCode keyCode) where T : ActiveItem
    {
        RemoveItem(keyCode);
        T item = gameObject.AddComponent<T>();
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
        projectile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(projectilePrefabPath);
        Projectile script = projectile.GetComponent<Projectile>();
        script.SetHostility(false);
        script.SetColor(GetComponent<SpriteRenderer>().color);
        script.SetSource(gameObject);
    }

    void Start()
    {
        GiveItem<PassiveItem_Weapon_0>();
        GetComponent<ConstraintInsideOfMap>().SetOffset(0.5f);
    }

    void Update()
    {
        healthBar.SetValue(health / maxHealth);
        // Move
        if (GetComponent<IMoveDisabled>() == null)
        {
            if (GetComponent<Waterblight>())
            {
                velocity = Vector3.MoveTowards(velocity, GetMoveSpeed() * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized, GetMoveSpeed() * Time.deltaTime / 0.5f);
            }
            else
            {
                velocity = GetMoveSpeed() * new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            }
            transform.Translate(Time.deltaTime * velocity);
        }
        foreach (KeyValuePair<KeyCode, ActiveItem> keyValuePair in activeItems.GetTUDict())
        {
            if (Input.GetKeyDown(keyValuePair.Key))
                keyValuePair.Value.Activate();
            if (Input.GetKeyUp(keyValuePair.Key))
                keyValuePair.Value.Deactivate();
        }
    }
}
