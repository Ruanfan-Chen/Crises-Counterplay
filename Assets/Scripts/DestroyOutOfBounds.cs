using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float xMin = -50.0f;
    private float xMax = 50.0f;
    private float yMin = -50.0f;
    private float yMax = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < xMin)
            Destroy(gameObject);
        if (transform.position.x > xMax)
            Destroy(gameObject);
        if (transform.position.y < yMin)
            Destroy(gameObject);
        if (transform.position.y > yMax)
            Destroy(gameObject);
    }
}
