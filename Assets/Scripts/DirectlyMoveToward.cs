using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectlyMoveToward : MonoBehaviour
{
    public GameObject target;
    private float speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

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
