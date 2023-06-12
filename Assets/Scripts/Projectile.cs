using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Projectile";
    private float speed;
    private bool hostility;
    public float GetSpeed() { return speed; }

    public void SetSpeed(float value) { speed = value; }

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Disposable";
        GetComponent<DestroyOutOfBounds>().SetOffset(-15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        return Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
    }

    public static GameObject Instantiate(Vector3 position, Vector3 lookAt)
    {
        return Instantiate(position, Quaternion.LookRotation(Vector3.forward, lookAt - position));
    }
    public static GameObject Instantiate(Vector3 position, float theta)
    {
        return Instantiate(position, Quaternion.Euler(0, 0, theta));
    }

    public static List<GameObject> InstantiateRing(Vector3 position, float theta, int count)
    {
        List<GameObject> projectileList = new();
        for (int i = 0; i < count; i++)
            projectileList.Add(Instantiate(position, theta + 360 * i / count ));
        return projectileList;
    }
}
