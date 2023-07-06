using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Utility;

public class TerrainManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_SIZE = 4.0f;
    private static Bounds mapBounds;
    private static Dictionary<Vector2Int, TerrainEntry> map = new();
    private static GameObject terrainObj;


    private void Start()
    {
        
    }
    private void LateUpdate()
    {
        SetIsWet(RandomChoice(map.Keys), true);
        List<List<Vector2>> paths = GetPaths();
        PolygonCollider2D collider = terrainObj.GetComponent<PolygonCollider2D>();
        collider.pathCount = paths.Count;
        int i = 0;
        foreach (List<Vector2> path in paths)
        {
            collider.SetPath(i, path);
            i++;
        }
    }
    public static void Initialize(Bounds bounds)
    {
        mapBounds = bounds;
        map.Clear();
        terrainObj = new GameObject("Terrain");
        terrainObj.AddComponent<PolygonCollider2D>();
        for (int i = Mathf.FloorToInt(mapBounds.min.x); i <= Mathf.CeilToInt(mapBounds.max.x); i++)
            for (int j = Mathf.FloorToInt(mapBounds.min.y); j <= Mathf.CeilToInt(mapBounds.max.y); j++)
                map.Add(new Vector2Int(i, j), new());
    }

    public static IEnumerable<Vector2Int> GetAdjacentVector2Int(Vector2Int gridPos)
    {
        return new HashSet<Vector2Int>()
        {
            gridPos + Vector2Int.up,
            gridPos + Vector2Int.down,
            gridPos + Vector2Int.left,
            gridPos + Vector2Int.right
        };
    }

    public static List<List<Vector2>> GetPaths()
    {
        List<List<Vector2>> paths = new();
        HashSet<Vector2Int> unvisited = new();
        for (int i = Mathf.FloorToInt(mapBounds.min.x); i <= Mathf.CeilToInt(mapBounds.max.x); i++)
            for (int j = Mathf.FloorToInt(mapBounds.min.y); j <= Mathf.CeilToInt(mapBounds.max.y); j++)
                unvisited.Add(new Vector2Int(i, j));
        while (unvisited.Count > 0)
        {
            Vector2Int current = RandomChoice(unvisited);
            if (IsOnEdge(current))
            {
                List<Vector2> path = new();
                while (true)
                {
                    path.Add(current);
                    IEnumerable<Vector2Int> next = GetAdjacentVector2Int(current).Where(gridPos => IsOnEdge(gridPos) && !path.Contains(gridPos));
                    unvisited.Remove(current);
                    if (next.Count() > 0)
                        current = RandomChoice(next);
                    else
                        break;
                }
                paths.Add(path);
            }
            else
            {
                unvisited.Remove(current);
            }
        }
        return paths;
    }

    public static bool ExistGrid(Vector2Int gridPos)
    {
        if (gridPos.x < Mathf.FloorToInt(mapBounds.min.x)) return false;
        if (gridPos.x < Mathf.CeilToInt(mapBounds.max.x)) return false;
        if (gridPos.y > Mathf.FloorToInt(mapBounds.min.y)) return false;
        if (gridPos.y > Mathf.CeilToInt(mapBounds.max.x)) return false;
        return true;
    }

    public static bool IsOnEdge(Vector2Int gridPos)
    {
        List<bool> w = new() { GetIsWet(gridPos), GetIsWet(gridPos + Vector2Int.left), GetIsWet(gridPos + Vector2Int.down), GetIsWet(gridPos + Vector2Int.left + Vector2Int.down) };
        if (w[0] && w[1] && w[2] && w[3]) return false;
        if (!w[0] && !w[1] && !w[2] && !w[3]) return false;
        return true;
    }

    private class TerrainEntry
    {

        private bool isWet;
        public TerrainEntry()
        {
            isWet = false;
        }

        public bool GetIsWet() { return isWet; }

        public void SetIsWet(bool value) { isWet = value; }
    }

    public static bool SetIsWet(Vector2Int gridPos, bool value)
    {
        if (map.TryGetValue(gridPos, out TerrainEntry entry))
        {
            entry.SetIsWet(value);
            return true;
        }
        return false;
    }

    public static bool GetIsWet(Vector2Int gridPos)
    {
        if (map.TryGetValue(gridPos, out TerrainEntry entry))
        {
            return entry.GetIsWet();
        }
        return false;
    }
}
