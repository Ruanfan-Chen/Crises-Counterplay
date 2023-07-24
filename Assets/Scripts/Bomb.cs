using UnityEngine;
using static Utility;

public class Bomb : MonoBehaviour
{
    private static readonly string prefabPath = "Prefabs/Bomb";
    private readonly float damage = 100.0f;
    private readonly float explosionDelay = 3.0f;
    private float timer;
    private GameObject source;
    private bool hostility;

    public void SetSource(GameObject value) => source = value;

    public void SetHostility(bool value) => hostility = value;

    // Start is called before the first frame update
    void Start()
    {
        timer = explosionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            foreach (GameObject g in OverlapGameObject(gameObject, collision => collision.GetComponent<IDamageable>() != null))
            {
                IDamageable damageable = g.GetComponent<IDamageable>();
                if (damageable.GetHostility() != hostility)
                {
                    new Damage(source, gameObject, damageable, damage, g.transform.position - transform.position).Apply();
                }
            }
            Destroy(gameObject);
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation, GameObject source, bool hostility)
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
        bomb.GetComponent<Bomb>().SetSource(source);
        bomb.GetComponent<Bomb>().SetHostility(hostility);
        return bomb;
    }
}
