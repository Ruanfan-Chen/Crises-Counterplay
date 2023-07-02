using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IProjectileModifier, IDamageable
{
    [SerializeField] private float health;
    private static float invDurationOnDmg = 0.7f;
    private static float knockbackDurationOnDmg = 0.5f;
    private static float knockbackDistanceOnDmg = 3.0f;
    private static float initialKnockbackSpeedOnDmg = 50.0f;
    [SerializeField] private float maxHealth;
    private List<PassiveItem> passiveItems = new();
    private ActiveItem activeItem = null;


    public float GetHealth() { return health; }

    public void SetHealth(float value) { health = value; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetMaxHealth(float value) { maxHealth = value; }

    public float GetMoveSpeed()
    {
        return GetComponentInParent<Player>().GetMoveSpeed();
    }

    public void SetMoveSpeed(float value)
    {
        GetComponentInParent<Player>().SetMoveSpeed(value);
    }
    public void ReceiveDamage(Damage damage)
    {
        if (GetComponent<Invulnerable>()) return;
        health -= damage.GetValue();
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        Vector3 sourcePos = transform.position;
        if (damage.GetSource())
            sourcePos = damage.GetSource().transform.position;
        if (damage.GetMedium())
            sourcePos = damage.GetMedium().transform.position;
        StartCoroutine(Utility.AddAndRemoveComponent(gameObject, typeof(Invulnerable), invDurationOnDmg));
        StartCoroutine(Utility.ForcedMovement(transform.parent, (transform.position - sourcePos).normalized * knockbackDistanceOnDmg, initialKnockbackSpeedOnDmg, knockbackDurationOnDmg));
    }
    public List<PassiveItem> GetPassiveItems() { return passiveItems; }
    public ActiveItem GetActiveItem() { return activeItem; }
    public bool RemoveItem(Component item)
    {
        if (activeItem == item)
        {
            activeItem = null;
            Destroy(item);
            return true;
        }
        else if (passiveItems.Remove((PassiveItem)item))
        {
            Destroy(item);
            return true;
        }
        return false;
    }
    public Component GiveItem(System.Type item)
    {
        if (typeof(ActiveItem).IsAssignableFrom(item))
        {
            if (activeItem) RemoveItem(activeItem);
            ActiveItem newComponent = (ActiveItem)gameObject.AddComponent(item);
            activeItem = newComponent;
            return newComponent;
        }
        if (typeof(PassiveItem).IsAssignableFrom(item))
        {
            PassiveItem newComponent = (PassiveItem)gameObject.AddComponent(item);
            passiveItems.Add(newComponent);
            return newComponent;
        }
        return null;

    }
    public void ActivateItem()
    {
        if (activeItem)
            activeItem.Activate();
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
        GiveItem(typeof(PassiveItem_Weapon_0));
    }
}
