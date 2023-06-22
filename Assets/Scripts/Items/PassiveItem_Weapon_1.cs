using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_Weapon_1 : PassiveItem, IProjectileModifier, IWeapon
{
    private GameObject view;
    private ViewBehavior viewScript;
    private PolygonCollider2D viewTrigger;
    private float knockbackDistance = 2.5f;
    private float knockbackSpeed = 5.0f;
    private float range = 10.0f;
    private float projectileSpeed = 10.0f;
    private float angleOfView = 120.0f;
    private int interpolationDensity = 4;

    public float GetDamage() { return 0.0f; }

    public void SetDamage(float value) { }

    public float GetKnockbackDistance() { return knockbackDistance; }

    public void SetKnockbackDistance(float value) { knockbackDistance = value; }

    public float GetKnockbackSpeed() { return knockbackSpeed; }
    public void SetKnockbackSpeed(float value) { knockbackSpeed = value; }

    public float GetRange() { return range; }

    public void SetRange(float value) { range = value; UpdateCollider(); }

    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }

    public float GetAngleOfView() { return angleOfView; }

    public void SetAngleOfView(float value) { angleOfView = value; UpdateCollider(); }

    public float GetAttackInterval() { return viewScript.GetAttackInterval(); }

    public void SetAttackInterval(float value) { viewScript.SetAttackInterval(value); }

    void Start()
    {
        view = new GameObject("WeaponView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        viewScript = view.AddComponent<ViewBehavior>();
        viewTrigger = view.AddComponent<PolygonCollider2D>();
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        List<Vector2> points = new();
        points.Add(Vector2.zero);
        for (float i = 0; i <= interpolationDensity; i++)
        {
            points.Add(range * (Quaternion.Euler(0, 0, (i / interpolationDensity - 0.5f) * angleOfView) * Vector3.up));
        }
        viewTrigger.SetPath(0, points);
    }

    void IProjectileModifier.Modify(GameObject projectile)
    {
        KnockbackOnCollision script = projectile.AddComponent<KnockbackOnCollision>();
        script.SetKnockbackDistance(knockbackDistance);
        script.SetKnockbackSpeed(knockbackSpeed);
        projectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        DestroyOutOfTime timer = projectile.AddComponent<DestroyOutOfTime>();
        timer.SetTimer(range / projectileSpeed);
        timer.Activate();
    }

    private class ViewBehavior : MonoBehaviour
    {
        private float timer = 0.0f;
        private float attackInterval = 0.5f;

        public float GetAttackInterval() { return attackInterval; }

        internal void SetAttackInterval(float value) { attackInterval = value; }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (timer > 0) return;
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != GetComponentInParent<Character>().GetHostility())
            {
                Projectile.Instantiate(transform.position, collision.transform.position, GetComponentsInParent<IProjectileModifier>());
                timer = attackInterval;
            }
        }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
        }
    }

    private class KnockbackOnCollision : MonoBehaviour
    {

        private float knockbackDistance;
        private float knockbackSpeed;

        public float GetKnockbackDistance() { return knockbackDistance; }

        public void SetKnockbackDistance(float value) { knockbackDistance = value; }

        public float GetKnockbackSpeed() { return knockbackSpeed; }
        public void SetKnockbackSpeed(float value) { knockbackSpeed = value; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Projectile script = GetComponent<Projectile>();
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != script.GetHostility())
            {
                damageable.StartCoroutine(Knockback(other.transform, transform.rotation * Vector3.up));
                Destroy(gameObject);
            }
        }

        IEnumerator Knockback(Transform other, Vector3 direction)
        {
            float cumulativeDisplacement = 0.0f;
            while (cumulativeDisplacement + knockbackSpeed * Time.deltaTime <= knockbackDistance)
            {
                cumulativeDisplacement += knockbackSpeed * Time.deltaTime;
                other.Translate(knockbackSpeed * Time.deltaTime * direction);
                yield return null;
            }
            other.Translate((knockbackDistance - cumulativeDisplacement) * direction);
        }
    }
}
