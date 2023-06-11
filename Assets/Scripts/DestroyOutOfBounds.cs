using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float offset = 0.0f;

    public float GetOffset() { return offset; }

    public void SetOffset(float value) { offset = value; }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("GameplayManager").GetComponent<MapManager>().IsInMap(transform.position, offset))
            Destroy(gameObject);
    }
}
