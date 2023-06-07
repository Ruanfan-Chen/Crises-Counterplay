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
        if (displacement.magnitude > speed * Time.deltaTime)
            transform.Translate(speed * Time.deltaTime * (target.transform.position - transform.position).normalized);
        else
            transform.Translate(displacement);
    }
}
