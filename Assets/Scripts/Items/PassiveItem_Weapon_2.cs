using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class PassiveItem_Weapon_2 : PassiveItem, IProjectileModifier, IWeapon
{
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Resources/Placeholder";
    private float damage = 100.0f;
    private float range = 6.0f;
    private float projectileSpeed = 5.0f;
    private float attackInterval = 2.0f;
    private float explosionDelay = 3.0f;
    private float explosionRadius = 3.0f;
    private float attackTimer = 0.0f;


    public float GetDamage() { return damage; }
    public void SetDamage(float value) { damage = value; }

    public float GetRange() { return range; }

    public void SetRange(float value) { range = value; }

    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }

    public float GetAngleOfView() { return 120.0f; }

    public void SetAngleOfView(float value) { }

    public float GetAttackInterval() { return attackInterval; }

    public void SetAttackInterval(float value) { attackInterval = value; }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Projectile.Instantiate(transform.position, transform.rotation, GetComponents<IProjectileModifier>());
            attackTimer = attackInterval;
        }
    }
    void IProjectileModifier.Modify(GameObject projectile)
    {
        projectile.AddComponent<CircleCollider2D>().radius = explosionRadius / projectile.transform.lossyScale.x;
        BombBehavior script = projectile.AddComponent<BombBehavior>();
        script.SetDamage(damage);
        script.SetAccelration(-projectileSpeed * projectileSpeed / range / 2.0f);
        script.SetTimer(explosionDelay);
        projectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
    }

    public static string GetDescription()
    {
        return description;
    }

    public static Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public static string GetName()
    {
        return itemName;
    }

    private class BombBehavior : MonoBehaviour
    {
        private float damage;
        private float explosionTimer;
        private float accelration;

        public float GetDamage() { return damage; }

        public void SetDamage(float value) { damage = value; }

        public float GetTimer() { return explosionTimer; }

        public void SetTimer(float value) { explosionTimer = value; }

        public float GetAccelration() { return accelration; }

        public void SetAccelration(float value) { accelration = value; }

        void Update()
        {
            Projectile script = GetComponent<Projectile>();
            float speed = script.GetSpeed();
            script.SetSpeed(Mathf.Clamp(speed + accelration * Time.deltaTime, 0.0f, float.MaxValue));
            explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0.0f)
            {
                foreach (GameObject g in OverlapGameObject(gameObject, collision => collision.GetComponent<IDamageable>() != null))
                {
                    IDamageable damageable = g.GetComponent<IDamageable>();
                    if (damageable.GetHostility() != script.GetHostility())
                    {
                        new Damage(script.GetSource(), gameObject, damageable, damage, g.transform.position - transform.position).Apply();
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}
