using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcTrigger : MonoBehaviour
{
    void Update()
    {
        GetComponent<CircleCollider2D>().radius =0.5f + ElectricArc.maxLength / transform.parent.lossyScale.x;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Vehicle>() != null)
            ElectricArc.Instantiate(collision.GetComponent<Collider2D>(), transform.parent.GetComponent<Collider2D>(), transform.parent.GetComponent<ElectricField>().GetDamage());
    }
}
