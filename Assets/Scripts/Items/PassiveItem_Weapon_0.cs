using UnityEngine;

public class PassiveItem_Weapon_0 : PassiveItem, IProjectileModifier, IWeapon
{
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Resources/Placeholder";
    private GameObject view;
    private ViewBehavior viewScript;
    private CircleCollider2D viewTrigger;
    private float damage = 25.0f;
    private float range = 10.0f;
    private float projectileSpeed = 10.0f;

    public float GetDamage() { return damage; }
    public void SetDamage(float value) { damage = value; }

    public float GetRange() { return range; }

    public void SetRange(float value) { range = value; UpdateCollider(); }

    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }

    public float GetAngleOfView() { return 360.0f; }

    public void SetAngleOfView(float value) { }

    public float GetAttackInterval() { return viewScript.GetAttackInterval(); }

    public void SetAttackInterval(float value) { viewScript.SetAttackInterval(value); }

    void OnEnable()
    {
        view = new GameObject("WeaponView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        viewScript = view.AddComponent<ViewBehavior>();
        viewTrigger = view.AddComponent<CircleCollider2D>();
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
        UpdateCollider();
    }
    void OnDisable()
    {
        Destroy(view);
        view = null;
        viewScript = null;
        viewTrigger = null;
    }

    private void UpdateCollider()
    {
        viewTrigger.radius = range;
    }

    void IProjectileModifier.Modify(GameObject projectile)
    {
        projectile.AddComponent<Projectile.DamageOnCollision>().SetDamage(damage);
        projectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        DestroyOutOfTime timer = projectile.AddComponent<DestroyOutOfTime>();
        timer.SetTimer(range / projectileSpeed);
        timer.Activate();
    }

    public override string GetDescription()
    {
        return description;
    }

    public override Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public override string GetName()
    {
        return itemName;
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
}
