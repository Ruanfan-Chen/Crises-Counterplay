using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectlyMoveToward : MonoBehaviour
{
    public GameObject target;
    private float speed = 2.5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = target.transform.position - transform.position;
        if (speed * Time.deltaTime < displacement.magnitude)
            transform.Translate(speed * Time.deltaTime * displacement.normalized);
        else
            transform.Translate(displacement);
    }
}
