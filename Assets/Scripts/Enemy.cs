using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IProjectileModifier
{
    private static string prefabPath = "Prefabs/Enemy";
    private float moveSpeed = 2.5f;
    private bool hostility = true;
    private float contactDPS = 25.00f;
    private float projectileSpeed = 2.5f;
    private float damage = 25.0f;
    private float range = 20.0f;

    public float GetMoveSpeed() { return moveSpeed; }

    public void SetMoveSpeed(float value) { moveSpeed = value; }

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    public float GetContactDPS() { return contactDPS; }

    public void SetContactDPS(float value) { contactDPS = value; }

    public void ReceiveDamage(Damage damage) { Destroy(gameObject); }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Disposable";
        GetComponent<ConstraintInsideOfMap>().SetOffset(0.5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable.GetHostility() != hostility)
        {
            new Damage(gameObject, null, damageable, contactDPS * Time.deltaTime).Apply();
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        return Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
    }

    public void Modify(GameObject projectile)
    {
        Projectile script = projectile.GetComponent<Projectile>();
        script.SetSpeed(projectileSpeed);
        script.SetHostility(true);
        script.SetColor(Color.black);
        script.SetSource(gameObject);
        script.SetDamage(damage);
        DestroyOutOfTime timer = projectile.AddComponent<DestroyOutOfTime>();
        timer.SetTimer(range / projectileSpeed);
        timer.Activate();
    }
}
