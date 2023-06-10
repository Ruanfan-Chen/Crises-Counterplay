using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject gameplayManager;
    private float health = 100.0f;
    private float maxHealth = 100.0f;
    private float projectileSpeed = 10.0f;
    private float attackInterval = 0.5f;
    private List<Component> passiveItems = new();
    private Component activeItem = null;
    private bool hostility;

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float value)
    {
        health = value;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
    }

    public float GetMoveSpeed()
    {
        return GetComponentInParent<PlayerControl>().GetSpeed();
    }

    public void SetMoveSpeed(float value)
    {
        GetComponentInParent<PlayerControl>().SetSpeed(value);
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public void SetProjectileSpeed(float value)
    {
        projectileSpeed = value;
    }

    public float GetAttackInterval()
    {
        return attackInterval;
    }

    public void SetAttackInterval(float value)
    {
        attackInterval = value;
    }

    public void ReceiveDmg(float value)
    {
        health -= value;
    }

    public Component GetActiveItem()
    {
        return activeItem;
    }

    public void RemoveActiveItem()
    {
        if (activeItem)
        {
            Destroy(activeItem);
            activeItem = null;
        }
    }
    public void SetActiveItem(System.Type item)
    {
        RemoveActiveItem();
        activeItem = gameObject.AddComponent(item);
    }

    public void AddPassiveItem(System.Type item)
    {
        passiveItems.Add(gameObject.AddComponent(item));
    }

    public List<Component> GetPassiveItems()
    {
        return passiveItems;
    }

    public void ActivateItem()
    {
        ((IItem)activeItem).Activate();
    }
    public bool GetHostility()
    {
        return hostility;
    }

    public void SetHostility(bool value)
    {
        hostility = value;
    }
}
