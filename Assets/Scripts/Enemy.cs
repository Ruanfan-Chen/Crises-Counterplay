using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private static string prefabPath = "Prefabs/Enemy";
    private bool hostility = true;
    private float contactDPS = 25.00f;

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
}
