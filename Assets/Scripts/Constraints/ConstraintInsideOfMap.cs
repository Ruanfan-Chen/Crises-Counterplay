using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintInsideOfMap : MonoBehaviour
{
    private float offset = 0.0f;

    public float GetOffset() { return offset; }

    public void SetOffset(float value) { offset = value; }

    private void LateUpdate()
    {

        transform.position = MapManager.PosInMap(transform.position, GetOffset());
    }
}
