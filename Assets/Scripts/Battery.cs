using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Battery";

    private static float VALUE = 5.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveItem_0 item = collision.GetComponent<ActiveItem_0>();
        if (item != null)
        {
            item.Charge(VALUE);
            Destroy(gameObject);
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject battery = Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
        battery.tag = "Disposable";
        return battery;
    }
}
