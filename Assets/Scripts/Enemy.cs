using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IProjectileModifier
{
    private static string prefabPath = "Prefabs/Enemy";
    private static string noShootPrefabPath = "Prefabs/EnemyNoShoot";
    public static readonly float minAttackInterval = 4.0f;
    public static readonly float maxAttackInterval = 5.0f;
    private float health = 50.0f;
    private float maxHealth = 50.0f;
    private float moveSpeed = 2.5f;
    private bool hostility = true;
    private float contactDamage = 25.0f;
    private float projectileSpeed = 2.5f;
    private float damage = 25.0f;
    private float range = float.PositiveInfinity;
    private Animator anim;
    [SerializeField] private Bar healthBar;

    public static float GetAttackInterval() => Random.Range(minAttackInterval, maxAttackInterval);

    public float GetMoveSpeed() { return moveSpeed; }

    public bool GetHostility() { return hostility; }

    public void ReceiveDamage(Damage damage)
    {
        health -= damage.GetValue();
        if (health <= 0 && health + damage.GetValue() > 0) Die();
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
            new Damage(gameObject, null, damageable, contactDamage, collision.transform.position - transform.position).Apply();
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation, System.Type[] components)
    {
        bool aggressive = false;
        foreach (System.Type componentType in components)
            if (typeof(EnemySpawn.IAggressive).IsAssignableFrom(componentType))
                aggressive = true;

        GameObject enemy = Instantiate(Resources.Load<GameObject>(aggressive ? prefabPath : noShootPrefabPath), position, rotation);
        enemy.tag = "Disposable";
        enemy.GetComponent<ConstraintInsideOfMap>().SetOffset(0.5f);
        foreach (System.Type componentType in components)
        {
            enemy.AddComponent(componentType);
        }

        Color color = Color.white;
        foreach (System.Type componentType in components)
            if (typeof(IOnDeathEffect).IsAssignableFrom(componentType))
                color = Color.red;

        enemy.GetComponent<SpriteRenderer>().color = color;
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

    void Update()
    {
        float value = health / maxHealth;
        if (health > 0.0f && health < maxHealth)
        {
            healthBar.SetIsHidden(false);
            healthBar.SetValue(health / maxHealth);
        }
        else
            healthBar.SetIsHidden(true);
    }
}
