using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IProjectileModifier
{
    private float health = 100.0f;
    private float maxHealth = 100.0f;
    private float projectileSpeed = 10.0f;
    private float range = 10.0f;
    private float angleOfView = 120.0f;
    private float attackInterval = 0.5f;
    private List<Component> passiveItems = new();
    private Component activeItem = null;


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

    public float GetRange() { return range; }

    public void SetRange(float value) { range = value; }

    public float GetAngleOfView() { return angleOfView; }

    public void SetAngleOfView(float value) { angleOfView = value; }

    public float GetAttackInterval() { return attackInterval; }

    public void SetAttackInterval(float value) { attackInterval = value; }

    public void ReceiveDmg(float value) { health -= value; }
    public List<Component> GetPassiveItems() { return passiveItems; }
    public Component GetActiveItem() { return activeItem; }
    public bool RemoveItem(Component item)
    {
        if (activeItem == item)
        {
            activeItem = null;
            Destroy(item);
            return true;
        }
        if (passiveItems.Remove(item))
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
            Component newComponent = gameObject.AddComponent(item);
            if (activeItem) RemoveItem(activeItem);
            activeItem = newComponent;
            return newComponent;
        }
        if (typeof(PassiveItem).IsAssignableFrom(item))
        {
            Component newComponent = gameObject.AddComponent(item);
            passiveItems.Add(newComponent);
            return newComponent;
        }
        return null;

    }
    public void ActivateItem()
    {
        GetComponent<ActiveItem>().Activate();
    }

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

    void IProjectileModifier.Modify(GameObject projectile)
    {
        projectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        projectile.GetComponent<Projectile>().SetHostility(false);
        projectile.GetComponent<Projectile>().SetColor(GetComponent<SpriteRenderer>().color);
        projectile.GetComponent<Projectile>().SetSource(gameObject);
        DestroyOutOfTime timer = projectile.AddComponent<DestroyOutOfTime>();
        timer.SetTimer(range / projectileSpeed);
        timer.Activate();
    }

    void Start()
    {
        GiveItem(typeof(PassiveItem_DefaultWeapon));
    }
}
