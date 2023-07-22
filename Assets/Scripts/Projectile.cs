using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Projectile";
    private float speed;
    private bool hostility;
    private GameObject source;

    public float GetSpeed() { return speed; }

    public void SetSpeed(float value) { speed = value; }

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    public Color GetColor() { return GetComponent<SpriteRenderer>().color; }

    public void SetColor(Color value) { GetComponent<SpriteRenderer>().color = value; }

    public GameObject GetSource() { return source; }

    public void SetSource(GameObject value) { source = value; }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation, IProjectileModifier[] modifiers)
    {
        GameObject projectile = Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
        projectile.tag = "Disposable";
        projectile.GetComponent<DestroyOutOfBounds>().SetOffset(-15.0f);
        foreach (IProjectileModifier modifier in modifiers)
            modifier.Modify(projectile);
        return projectile;
    }

    public static GameObject Instantiate(Vector3 position, Vector3 lookAt, IProjectileModifier[] modifiers)
    {
        return Instantiate(position, Quaternion.LookRotation(Vector3.forward, lookAt - position), modifiers);
    }
    public static GameObject Instantiate(Vector3 position, float theta, IProjectileModifier[] modifiers)
    {
        return Instantiate(position, Quaternion.Euler(0, 0, theta), modifiers);
    }

    public static List<GameObject> InstantiateRing(Vector3 position, float theta, IProjectileModifier[] modifiers, int count)
    {
        List<GameObject> projectileList = new();
        for (int i = 0; i < count; i++)
            projectileList.Add(Instantiate(position, theta + 360 * i / count, modifiers));
        return projectileList;
    }

    public class DamageOnCollision : MonoBehaviour
    {
        private float damage;

        public DamageOnCollision(float damage)
        {
            this.damage = damage;
        }

        public float GetDamage() { return damage; }

        public void SetDamage(float value) { damage = value; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Projectile script = GetComponent<Projectile>();
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != script.GetHostility())
            {
                new Damage(script.GetSource(), gameObject, damageable, damage, transform.rotation * Vector3.up).Apply();
                Destroy(gameObject);
            }
        }
    }
}
