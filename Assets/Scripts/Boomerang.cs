using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private static readonly string prefabPath = "Prefabs/Boomerang";
    private readonly float angularVelocity = 360.0f;
    private readonly float contactDPS = 100.0f;
    private GameObject source;
    private bool hostility;

    public void SetSource(GameObject value) => source = value;

    public void SetHostility(bool value) => hostility = value;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, Time.deltaTime * angularVelocity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable.GetHostility() != hostility)
        {
            new Damage(source, gameObject, damageable, contactDPS * Time.deltaTime, Vector3.zero).Apply();
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation, GameObject source, bool hostility)
    {
        GameObject boomerang = Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
        boomerang.GetComponent<Boomerang>().SetSource(source);
        boomerang.GetComponent<Boomerang>().SetHostility(hostility);
        return boomerang;
    }
}
