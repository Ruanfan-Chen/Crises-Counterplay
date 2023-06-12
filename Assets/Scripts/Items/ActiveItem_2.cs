using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_2 : ActiveItem
{
    private float dashSpeed = 50.0f;
    private float displacement = 10.0f;
    public override void Activate()
    {
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        float cumulativeDisplacement = 0.0f;
        while (cumulativeDisplacement + dashSpeed * Time.deltaTime <= displacement)
        {
            cumulativeDisplacement += dashSpeed * Time.deltaTime;
            GetComponentInParent<Player>().transform.Translate(dashSpeed * Time.deltaTime * (transform.rotation * Vector3.up));
            yield return null;
        }
        GetComponentInParent<Player>().transform.Translate(transform.rotation * Vector3.up * (displacement - cumulativeDisplacement));
    }
}
