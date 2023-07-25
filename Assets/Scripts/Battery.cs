using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Battery";
    private static string tracePrefabPath = "Prefabs/BatteryTrace";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveItem_0 item = collision.GetComponent<ActiveItem_0>();
        if (item != null)
        {
            item.Charge();
            Destroy(gameObject);
        }
    }

    public static GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject battery = Instantiate(Resources.Load<GameObject>(prefabPath), position, rotation);
        battery.tag = "Disposable";
        return battery;
    }

    public static GameObject InstantiateTrace(Vector3 position, Quaternion rotation)
    {
        GameObject batteryTrace = Instantiate(Resources.Load<GameObject>(tracePrefabPath), position, rotation);
        batteryTrace.tag = "Disposable";
        return batteryTrace;
    }
}
