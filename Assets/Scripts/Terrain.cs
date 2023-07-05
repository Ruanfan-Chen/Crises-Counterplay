using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    Vector2Int gridPos;

    public Vector2Int GetGridPos() { return gridPos; }

    public void SetGridPos(Vector2Int value) { gridPos = value; }
}
