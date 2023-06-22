using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IProjectileModifier, IDamageable
{
    [SerializeField] private float health = 100.0f;
    [SerializeField] private GameplayManager gameplayManager;
    private float maxHealth = 100.0f;
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
        health -= damage.GetValue();
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        if(health <= 0.0f)
        {
            gameplayManager.ResetGame(1);
        }
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
        GetComponent<ActiveItem>().Activate();
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
        GiveItem(typeof(PassiveItem_Weapon_3));
    }
}
