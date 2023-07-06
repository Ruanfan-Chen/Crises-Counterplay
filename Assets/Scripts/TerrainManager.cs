using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_SIZE = 1.0f;
    private static readonly float sqrt3 = Mathf.Sqrt(3.0f);
    private static Bounds mapBounds;
    private static readonly List<List<TerrainData>> terrainI = new();
    private static readonly List<List<TerrainData>> terrainII = new();
    private static readonly List<List<TerrainData>> terrainIII = new();
    private static readonly List<List<TerrainData>> terrainIV = new();

    public static void Initialize(Bounds bounds)
    {
        mapBounds = bounds;
        terrainI.Clear();
        terrainII.Clear();
        terrainIII.Clear();
        terrainIV.Clear();
        Queue<Vector2Int> queue = new();
        queue.Enqueue(Vector2Int.zero);
        HashSet<Vector2Int> explored = new() { Vector2Int.zero };
        while (queue.Count > 0)
        {
            Vector2Int currentGrid = queue.Dequeue();
            switch (currentGrid.x, currentGrid.y)
            {
                case ( >= 0, >= 0):
                    terrainI[currentGrid.x][currentGrid.y] = new();
                    break;
                case ( < 0, >= 0):
                    terrainII[-currentGrid.x - 1][currentGrid.y] = new();
                    break;
                case ( < 0, < 0):
                    terrainIII[-currentGrid.x - 1][-currentGrid.y - 1] = new();
                    break;
                case ( >= 0, < 0):
                    terrainIV[currentGrid.x][-currentGrid.y - 1] = new();
                    break;
            }
            foreach (Vector2Int neighbour in GetNeighbourGrid(currentGrid))
            {
                if (!explored.Contains(neighbour))
                {
                    explored.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
    }

    public static TerrainData GetTerrainData(Vector2Int gridPos)
    {
        switch (gridPos.x, gridPos.y)
        {
            case ( >= 0, >= 0):
                return terrainI[gridPos.x][gridPos.y];
            case ( < 0, >= 0):
                return terrainII[-gridPos.x - 1][gridPos.y];
            case ( < 0, < 0):
                return terrainIII[-gridPos.x - 1][-gridPos.y - 1];
            case ( >= 0, < 0):
                return terrainIV[gridPos.x][-gridPos.y - 1];
        }
    }

    public static IEnumerable<Vector2Int> GetNeighbourGrid(Vector2Int gridPos)
    {
        return new HashSet<Vector2Int>()
        {
            gridPos + Vector2Int.left * 2,
            gridPos + Vector2Int.left,
            gridPos + Vector2Int.right,
            gridPos + Vector2Int.right * 2,
            gridPos + Vector2Int.left + (gridPos.x - 1) % 2 * Vector2Int.down,
            gridPos + Vector2Int.right + (gridPos.x + 1) % 2 * Vector2Int.down
        }.Where(gridPos => ExistGrid(gridPos));
    }

    public static Vector2 GetPosition2D(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * UNIT_SIZE * sqrt3 / 2.0f, (gridPos.y * 2 + gridPos.x % 2) * UNIT_SIZE);
    }

    public static Vector3 GetPosition3D(Vector2Int gridPos)
    {
        Vector2 position2 = GetPosition2D(gridPos);
        return new Vector3(position2.x, position2.y, TERRAIN_DEPTH);
    }

    public static bool ExistGrid(Vector2Int gridPos)
    {
        return mapBounds.SqrDistance(GetPosition2D(gridPos)) <= UNIT_SIZE * UNIT_SIZE;
    }

    public class TerrainData
    {
        private bool isWet;
        public TerrainData()
        {
            isWet = false;
        }
    }
}
