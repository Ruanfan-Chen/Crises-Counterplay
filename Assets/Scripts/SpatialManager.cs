using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class SpatialManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_RADIUS = 1.0f;
    public static readonly Vector2 BASE_X = new(0.5f * Mathf.Sqrt(3.0f), 0.5f);
    public static readonly Vector2 BASE_Y = new(0.0f, 1.0f);
    public static readonly Vector2Int UP = new(0, 1);
    public static readonly Vector2Int UPLEFT = new(-1, 1);
    public static readonly Vector2Int UPRIGHT = new(1, 0);
    public static readonly Vector2Int DOWN = new(0, -1);
    public static readonly Vector2Int DOWNLEFT = new(-1, 0);
    public static readonly Vector2Int DOWNRIGHT = new(1, -1);
    private static readonly Dictionary<string, GameObject> layers = new();

    public static void UpdateLayerDisplay<T>(string layerName, int interpolationDensity, Func<T, bool> predicate)
    {
        List<List<Vector2>> paths = GetContourLines(layerName, interpolationDensity, predicate);
        PolygonCollider2D collider = layers[layerName].GetComponent<PolygonCollider2D>();
        collider.pathCount = paths.Count;
        for (int i = 0; i < paths.Count; i++)
            collider.SetPath(i, paths[i]);
        layers[layerName].GetComponent<MeshFilter>().mesh = collider.CreateMesh(false, false);
    }
    public static void Initialize()
    {
        SpatialData.Clear();
        foreach (GameObject layer in layers.Values)
        {
            Destroy(layer);
        }
        layers.Clear();
    }
    public static bool IsDataPoint(Vector2Int hivePos)
    {
        return (hivePos.x - hivePos.y) % 3 == 0;
    }

    public static Vector2Int GetClosestDataPoint(Vector2 cartesian)
    {
        float x = Vector3.Cross(cartesian, BASE_Y).z / Vector3.Cross(BASE_X, BASE_Y).z / UNIT_RADIUS;
        float y = Vector3.Cross(BASE_X, cartesian).z / Vector3.Cross(BASE_X, BASE_Y).z / UNIT_RADIUS;
        Vector2Int quotient = new(Mathf.FloorToInt(x / 3.0f), Mathf.FloorToInt(y / 3.0f));
        Vector2 reminder = new Vector2(x, y) - quotient * 3;
        if (reminder.x < 1.0f || reminder.y >= 2.0f)
            return quotient * 3 + new Vector2Int(0, 3);
        if (reminder.x >= 2.0f || reminder.y < 1.0f)
            return quotient * 3 + new Vector2Int(3, 0);
        return (reminder.x + reminder.y) switch
        {
            < 1.0f => quotient * 3,
            < 3.0f => (quotient * 3) + new Vector2Int(1, 1),
            < 5.0f => (quotient * 3) + new Vector2Int(2, 2),
            _ => (quotient * 3) + new Vector2Int(3, 3),
        };
    }

    public static Vector2 Hive2Cartesian(Vector2 hivePos)
    {
        return hivePos.x * UNIT_RADIUS * BASE_X + hivePos.y * UNIT_RADIUS * BASE_Y;
    }

    public static Vector2Int GetTangent<T>(string layerName, Vector2Int hivePos, Func<T, bool> predicate)
    {
        switch ((hivePos.x - hivePos.y) % 3)
        {
            case 0:
                return Vector2Int.zero;
            case 1 or -2:
                switch (
                    predicate(SpatialData.GetValue<T>(layerName, hivePos + UP)),
                    predicate(SpatialData.GetValue<T>(layerName, hivePos + DOWNLEFT)),
                    predicate(SpatialData.GetValue<T>(layerName, hivePos + DOWNRIGHT)))
                {
                    case (false, false, false):
                        return Vector2Int.zero;
                    case (false, false, true):
                        return DOWN;
                    case (false, true, false):
                        return UPLEFT;
                    case (false, true, true):
                        return UPLEFT;
                    case (true, false, false):
                        return UPRIGHT;
                    case (true, false, true):
                        return DOWN;
                    case (true, true, false):
                        return UPRIGHT;
                    case (true, true, true):
                        return Vector2Int.zero;
                }
            case 2 or -1:
                switch (
                    predicate(SpatialData.GetValue<T>(layerName, hivePos + DOWN)),
                    predicate(SpatialData.GetValue<T>(layerName, hivePos + UPLEFT)),
                    predicate(SpatialData.GetValue<T>(layerName, hivePos + UPRIGHT)))
                {
                    case (false, false, false):
                        return Vector2Int.zero;
                    case (false, false, true):
                        return DOWNRIGHT;
                    case (false, true, false):
                        return UP;
                    case (false, true, true):
                        return DOWNRIGHT;
                    case (true, false, false):
                        return DOWNLEFT;
                    case (true, false, true):
                        return DOWNLEFT;
                    case (true, true, false):
                        return UP;
                    case (true, true, true):
                        return Vector2Int.zero;
                }
        }
        throw new Exception();
    }

    public static List<List<Vector2>> GetContourLines<T>(string layerName, int interpolationDensity, Func<T, bool> predicate)
    {
        HashSet<Vector2Int> vertices = new();
        foreach (SpatialData entity in SpatialData.GetAllNonDefaultDatapoint().Values)
        {
            if (!predicate(entity.GetValue<T>(layerName)))
                continue;
            vertices.Add(entity.hivePos + UP);
        }

        List<List<Vector2>> paths = new();

        while (vertices.Count > 0)
        {
            Vector2Int startPos = RandomChoice(vertices); ;
            List<Vector2> smoothLine = new();
            Vector2Int prev = startPos;
            Vector2Int current = prev + GetTangent(layerName, prev, predicate);
            Vector2Int next = current + GetTangent(layerName, current, predicate);
            bool IsStraightLine = false;
            do
            {
                vertices.Remove(current);
                if (prev + next == current * 2)
                {
                    if (!IsStraightLine)
                        smoothLine.Add((Hive2Cartesian(prev + current)) / 2.0f);
                    IsStraightLine = true;
                }
                else
                {
                    for (int j = 0; j < interpolationDensity; j++)
                    {
                        float t = (float)j / interpolationDensity;
                        smoothLine.Add(Hive2Cartesian(Vector2.Lerp(prev + current, current + next, t)) / 2.0f);
                    }
                    IsStraightLine = false;
                }
                prev = current;
                current = next;
                next = current + GetTangent(layerName, current, predicate);
            } while (prev != startPos);
            if (smoothLine.Count > interpolationDensity)
                paths.Add(smoothLine);
        }
        return paths;
    }

    public class SpatialData
    {
        private static readonly Dictionary<Vector2Int, SpatialData> datapoints = new();
        public readonly Vector2Int hivePos;
        private readonly Dictionary<string, object> data = new();

        private SpatialData(Vector2Int hivePos)
        {
            this.hivePos = hivePos;
            datapoints.Add(hivePos, this);
        }

        public void SetValue(string layerName, object value)
        {
            if (!layers.ContainsKey(layerName))
            {
                GameObject layer = new GameObject(layerName + "Layer");
                layer.transform.position = Vector3.forward * TERRAIN_DEPTH;
                layer.AddComponent<MeshRenderer>();
                layer.AddComponent<MeshFilter>();
                layer.AddComponent<PolygonCollider2D>();
                layers.Add(layerName, layer);
            }
            if (data.ContainsKey(layerName))
                data[layerName] = value;
            else
                data.Add(layerName, value);
        }
        public T GetValue<T>(string layerName)
        {
            return data.ContainsKey(layerName) ? (T)data[layerName] : default;
        }
        public static void SetValue(string layerName, Vector2Int hivePos, object value)
        {
            if (!IsDataPoint(hivePos)) throw new ArgumentException("Cannot set value on a vertex.");
            SpatialData datapoint = (datapoints.ContainsKey(hivePos)) ? datapoints[hivePos] : new SpatialData(hivePos);
            datapoint.SetValue(layerName, value);
        }

        public static T GetValue<T>(string layerName, Vector2Int hivePos)
        {
            if (!(IsDataPoint(hivePos) && datapoints.ContainsKey(hivePos))) return default;
            return datapoints[hivePos].GetValue<T>(layerName);
        }

        public static IReadOnlyDictionary<Vector2Int, SpatialData> GetAllNonDefaultDatapoint() { return datapoints; }

        public static void Clear()
        {
            datapoints.Clear();
        }
    }
}
