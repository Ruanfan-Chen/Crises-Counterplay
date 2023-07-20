using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IProjectileModifier
{
    private static string prefabPath = "Prefabs/Enemy";
    private float health = 50.0f;
    private float maxHealth = 50.0f;
    private float moveSpeed = 2.5f;
    private bool hostility = true;
    private float contactDamage = 25.0f;
    private float projectileSpeed = 2.5f;
    private float damage = 25.0f;
    private float range = float.PositiveInfinity;
    Animator anim;

    public float GetHealth() { return health; }

    public void SetHealth(float value) { health = value; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetMaxHealth(float value) { maxHealth = value; }

    public float GetMoveSpeed() { return moveSpeed; }

    public void SetMoveSpeed(float value) { moveSpeed = value; }

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    public float GetContactDamage() { return contactDamage; }

    public void SetContactDamage(float value) { contactDamage = value; }

    public float GetRange() { return range; }

    public void SetRange(float value) { range = value; }

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }

    public void ReceiveDamage(Damage damage)
    {
        health -= damage.GetValue();
        if (health <= 0) Die();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Die()
    {
        foreach (IOnDeathEffect component in GetComponents<IOnDeathEffect>())
            component.OnDeath();
        anim.SetBool("isDead", true);
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable.GetHostility() != hostility)
        {
            new Damage(gameObject, null, damageable, contactDamage).Apply();
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject enemy = Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
        enemy.tag = "Disposable";
        enemy.GetComponent<ConstraintInsideOfMap>().SetOffset(0.5f);
        return enemy;
    }

    void IProjectileModifier.Modify(GameObject projectile)
    {
        projectile.AddComponent<Projectile.DamageOnCollision>().SetDamage(damage);
        Projectile script = projectile.GetComponent<Projectile>();
        script.SetSpeed(projectileSpeed);
        script.SetHostility(true);
        script.SetSource(gameObject);
        if (float.IsFinite(range / projectileSpeed))
            Destroy(projectile, range / projectileSpeed);
    }
}
