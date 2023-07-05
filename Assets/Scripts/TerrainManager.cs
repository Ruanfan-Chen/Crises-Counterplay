using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_SIZE = 3.0f;
    private static readonly float sqrt3 = Mathf.Sqrt(3);

    public void Initialize(Bounds bounds)
    {
    }

    private GameObject InstantiateUnit(Vector2Int girdPos)
    {
        GameObject unit = new();
        unit.transform.position = GetPosition3D(girdPos);
        return unit;
    }

    public static Vector2 GetPosition2D(Vector2Int girdPos)
    {
        return new Vector2(girdPos.x * UNIT_SIZE * sqrt3 / 2.0f, girdPos.y * UNIT_SIZE + (x % 2) * sqrt3);
    }

    public static Vector3 GetPosition3D(Vector2Int girdPos)
    {
        Vector2 position2 = GetPosition2D(girdPos);
        return new Vector3(position2.x, position2.y, TERRAIN_DEPTH);
    }
}
