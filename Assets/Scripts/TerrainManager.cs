using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using static Utility;

public class TerrainManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_SIZE = 1.0f;
    private static Bounds mapBounds;
    private static Dictionary<Vector2Int, TerrainEntry> map = new();
    private static ISet<Edge> edges = new HashSet<Edge>();
    public static void Initialize(Bounds bounds)
    {
        mapBounds = bounds;
        map.Clear();
        edges.Clear();
        foreach (Vector2Int gridPos in BFTraversal(Vector2Int.zero, gridPos => GetAdjacentGrid(gridPos).Where(gridPos => ExistGrid(gridPos))))
            map.Add(gridPos, new(gridPos));
    }

    public static IEnumerable<Vector2Int> GetAdjacentGrid(Vector2Int gridPos)
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
        Dictionary<Edge, int> curl = new();
        foreach (Edge e in edges)
        {
            curl.Add(e, 0);
        }
        foreach (TerrainEntry terrainEntry in map.Values)
        {
            curl[terrainEntry.topEdge]++;
            curl[terrainEntry.bottomEdge]--;
            curl[terrainEntry.leftEdge]++;
            curl[terrainEntry.rightEdge]--;
        }
        List<List<Vector2>> paths = new();
        while (curl.Count > 0)
        {
            KeyValuePair<Edge, int> kvp = curl.First();
            if (kvp.Value == 0)
            {
                curl.Remove(kvp.Key);
                continue;
            }
            List<Vector2> path = new();
            paths.Add(path);
            foreach (Edge edge in BFTraversal(kvp.Key, e => e.GetAdjcent().Where(e => curl[e] != 0)))
            {

                curl.Remove(edge);
                path.Add(edge.GetMidpointGridPos());
            }
        };
        return paths;
    }

    public static bool ExistGrid(Vector2Int gridPos)
    {
        return mapBounds.SqrDistance((Vector2)gridPos) * 4.0f <= UNIT_SIZE * UNIT_SIZE;
    }

    private class TerrainEntry
    {
        public readonly Vertex topLeftVertex;
        public readonly Vertex topRightVertex;
        public readonly Vertex bottomLeftVertex;
        public readonly Vertex bottomRightVertex;
        public readonly Edge topEdge;
        public readonly Edge bottomEdge;
        public readonly Edge leftEdge;
        public readonly Edge rightEdge;

        private bool isWet;
        public TerrainEntry(Vector2Int gridPos)
        {
            isWet = false;
            if (map.TryGetValue(gridPos + Vector2Int.up, out TerrainEntry topEntry))
            {
                topLeftVertex = topEntry.bottomLeftVertex;
                topRightVertex = topEntry.bottomRightVertex;
            }
            if (map.TryGetValue(gridPos + Vector2Int.down, out TerrainEntry downEntry))
            {
                bottomLeftVertex = downEntry.topLeftVertex;
                bottomRightVertex = downEntry.topRightVertex;
            }
            if (map.TryGetValue(gridPos + Vector2Int.left, out TerrainEntry leftEntry))
            {
                topLeftVertex = leftEntry.topRightVertex;
                bottomLeftVertex = leftEntry.bottomRightVertex;
            }
            if (map.TryGetValue(gridPos + Vector2Int.right, out TerrainEntry rightEntry))
            {
                topRightVertex = rightEntry.topLeftVertex;
                bottomRightVertex = rightEntry.bottomLeftVertex;
            }
            if (topLeftVertex == null)
            {
                if (map.TryGetValue(gridPos + Vector2Int.up + Vector2Int.left, out TerrainEntry topLeftEntry))
                    topLeftVertex = topLeftEntry.bottomRightVertex;
                else
                    topLeftVertex = new(gridPos + Vector2.up / 2.0f + Vector2.left / 2.0f);
            }
            if (topRightVertex == null)
            {
                if (map.TryGetValue(gridPos + Vector2Int.up + Vector2Int.right, out TerrainEntry topRightEntry))
                    topRightVertex = topRightEntry.bottomLeftVertex;
                else
                    topRightVertex = new(gridPos + Vector2.up / 2.0f + Vector2.right / 2.0f);
            }
            if (bottomLeftVertex == null)
            {
                if (map.TryGetValue(gridPos + Vector2Int.down + Vector2Int.left, out TerrainEntry bottomLeftEntry))
                    bottomLeftVertex = bottomLeftEntry.topRightVertex;
                else
                    bottomLeftVertex = new(gridPos + Vector2.down / 2.0f + Vector2.left / 2.0f);
            }
            if (bottomRightVertex == null)
            {
                if (map.TryGetValue(gridPos + Vector2Int.down + Vector2Int.right, out TerrainEntry bottomRightEntry))
                    bottomRightVertex = bottomRightEntry.topLeftVertex;
                else
                    bottomRightVertex = new(gridPos + Vector2.down / 2.0f + Vector2.right / 2.0f);
            }
            if (map.TryGetValue(gridPos + Vector2Int.up, out topEntry))
                topEdge = topEntry.bottomEdge;
            else
                topEdge = new(false, topLeftVertex, topRightVertex);
            if (map.TryGetValue(gridPos + Vector2Int.down, out downEntry))
                bottomEdge = downEntry.topEdge;
            else
                bottomEdge = new(false, bottomLeftVertex, bottomRightVertex);
            if (map.TryGetValue(gridPos + Vector2Int.left, out leftEntry))
                leftEdge = leftEntry.rightEdge;
            else
                leftEdge = new(true, topLeftVertex, bottomLeftVertex);
            if (map.TryGetValue(gridPos + Vector2Int.right, out rightEntry))
                rightEdge = rightEntry.leftEdge;
            else
                rightEdge = new(true, topRightVertex, bottomRightVertex);
        }

        public bool GetIsWet() { return isWet; }

        public void SetIsWet(bool value) { isWet = value; }
    }

    private class Edge
    {
        public readonly bool isVertical;
        public readonly Vertex topOrLeftVertex;
        public readonly Vertex bottomOrRighrVertex;

        public Edge(bool isVertical, Vertex topOrLeftVertex, Vertex bottomOrRighrVertex)
        {
            this.isVertical = isVertical;
            this.topOrLeftVertex = topOrLeftVertex;
            this.bottomOrRighrVertex = bottomOrRighrVertex;
            edges.Add(this);
        }

        public Vector2 GetMidpointGridPos()
        {
            return (topOrLeftVertex.gridPos + bottomOrRighrVertex.gridPos) / 2.0f;
        }

        public IEnumerable<Edge> GetAdjcent()
        {
            HashSet<Edge> adj = new() { };
            adj.Add(topOrLeftVertex.GetTopEdge());
            adj.Add(topOrLeftVertex.GetBottomEdge());
            adj.Add(topOrLeftVertex.GetLeftEdge());
            adj.Add(topOrLeftVertex.GetRightEdge());
            adj.Add(bottomOrRighrVertex.GetTopEdge());
            adj.Add(bottomOrRighrVertex.GetBottomEdge());
            adj.Add(bottomOrRighrVertex.GetLeftEdge());
            adj.Add(bottomOrRighrVertex.GetRightEdge());
            adj.Remove(this);
            return adj;
        }
    }

    private class Vertex
    {
        private Edge topEdge;
        private Edge bottomEdge;
        private Edge leftEdge;
        private Edge rightEdge;
        public readonly Vector2 gridPos;

        public Edge GetTopEdge() { return topEdge; }

        public void SetTopEdge(Edge value) { topEdge = value; }

        public Edge GetBottomEdge() { return bottomEdge; }

        public void SetBottomEdge(Edge value) { bottomEdge = value; }

        public Edge GetLeftEdge() { return leftEdge; }

        public void SetLeftEdge(Edge value) { leftEdge = value; }

        public Edge GetRightEdge() { return rightEdge; }

        public void SetRightEdge(Edge value) { rightEdge = value; }

        public Vertex(Vector2 gridPos)
        {
            this.gridPos = gridPos;
        }
    }
}
