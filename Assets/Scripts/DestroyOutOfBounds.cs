using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private System.Func<Vector3, float, bool> isInMap;
    private float offset = -15.0f;
    private Vector3 min;
    private Vector3 max;

    public Func<Vector3, float, bool> GetIsInMap()
    {
        return isInMap;
    }

    public void SetIsInMap(Func<Vector3, float, bool> value)
    {
        isInMap = value;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isInMap(transform.position, offset))
            Destroy(gameObject);
    }
}
