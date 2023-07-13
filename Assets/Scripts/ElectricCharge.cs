using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricCharge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveItem_0 item = collision.gameObject.GetComponent<ActiveItem_0>();
        if (item)
            collision.gameObject.AddComponent<Charging>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Charging script = collision.gameObject.GetComponent<Charging>();
        if (script)
            Destroy(script);
    }

    public class Charging : MonoBehaviour
    {
    }
}
