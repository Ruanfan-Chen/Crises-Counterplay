using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float health = 100.0f;
    private float maxHealth = 100.0f;
    private float projectileSpeed = 10.0f;
    private float attackInterval = 0.5f;
    private List<Component> passiveItems = new();
    private Component activeItem = null;
    private bool hostility = false;

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

    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }

    public float GetAttackInterval() { return attackInterval; }

    public void SetAttackInterval(float value) { attackInterval = value; }

    public void ReceiveDmg(float value) { health -= value; }
    public List<Component> GetPassiveItems() { return passiveItems; }
    public void RemovePassiveItem(Component item)
    {
        passiveItems.Remove(item);
        Destroy(item);
    }
    public void AddPassiveItem(System.Type item)
    {
        passiveItems.Add(gameObject.AddComponent(item));
    }
    public Component GetActiveItem() { return activeItem; }

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
    public void ActivateItem()
    {
        if (activeItem)
        {
            ((IItem)activeItem).Activate();
        }
    }
    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (!collision.isTrigger && !GetComponent<Faction>().IsFriendly(collision.GetComponent<Faction>()))
        {
            if (collision.GetComponent<ProjectileMove>())
            {

                GetComponent<Character>().ReceiveDmg(25.0f);
                Destroy(collision.gameObject);
            }
            else
                GetComponent<Character>().ReceiveDmg(25.0f * Time.deltaTime);
        }*/
    }

    private void Start()
    {
        //SetActiveItem(typeof(ActiveItem_1));
    }
}
