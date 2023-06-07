using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public GameObject map;
    private float offset = 15.0f;
    private Vector3 min;
    private Vector3 max;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        min = map.transform.position - map.transform.lossyScale / 2;
        max = map.transform.position + map.transform.lossyScale / 2;
        if (transform.position.x < min.x - offset)
            Destroy(gameObject);
        if (transform.position.x > max.x + offset)
            Destroy(gameObject);
        if (transform.position.y < min.y - offset)
            Destroy(gameObject);
        if (transform.position.y > max.y + offset)
            Destroy(gameObject);
    }
}
