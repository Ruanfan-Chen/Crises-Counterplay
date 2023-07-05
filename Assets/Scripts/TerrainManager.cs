using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static Utility;

public class TerrainManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_SIZE = 1.0f;
    private static readonly float sqrt3 = Mathf.Sqrt(3);
    private BiDictionary<Vector2Int, GameObject> terrainGraph = new();

    public void Initialize(Bounds bounds)
    {
        Queue<Vector2Int> queue = new();
        queue.Enqueue(Vector2Int.zero);
        HashSet<Vector2Int> explored = new() { Vector2Int.zero };
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            InstantiateUnit(current);
            foreach (Vector2Int neighbour in GetNeighbour(current).Where(gridPos => IsInBounds(gridPos, bounds)))
            {
                if (!explored.Contains(neighbour))
                {
                    explored.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
    }

    private GameObject InstantiateUnit(Vector2Int gridPos)
    {
        GameObject unit = new("Terrain" + gridPos.x + gridPos.y);
        unit.transform.position = GetPosition3D(gridPos);
        unit.AddComponent<Terrain>().SetGridPos(gridPos);
        terrainGraph.Add(gridPos, unit);
        return unit;
    }

    public static IEnumerable<Vector2Int> GetNeighbour(Vector2Int gridPos)
    {
        return new HashSet<Vector2Int>()
        {
            gridPos + Vector2Int.left * 2,
            gridPos + Vector2Int.left,
            gridPos + Vector2Int.right,
            gridPos + Vector2Int.right * 2,
            gridPos + Vector2Int.left - (gridPos.x - 1) % 2 * Vector2Int.up,
            gridPos + Vector2Int.right - (gridPos.x + 1) % 2 * Vector2Int.up
        };
    }

    public static Vector2 GetPosition2D(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * UNIT_SIZE * sqrt3 / 2.0f, (gridPos.y * 2 + gridPos.x % 2) * UNIT_SIZE );
    }

    public static Vector3 GetPosition3D(Vector2Int gridPos)
    {
        Vector2 position2 = GetPosition2D(gridPos);
        return new Vector3(position2.x, position2.y, TERRAIN_DEPTH);
    }

    public static bool IsInBounds(Vector2Int gridPos, Bounds bounds)
    {
        return bounds.SqrDistance(GetPosition2D(gridPos)) <= UNIT_SIZE * UNIT_SIZE;
    }
}
