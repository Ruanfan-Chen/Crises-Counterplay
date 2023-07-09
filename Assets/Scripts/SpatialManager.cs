using System;
using System.Collections.Generic;
using UnityEngine;

public class SpatialManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    public static readonly float UNIT_RADIUS = 0.5f;
    public static readonly Vector2 BASE_X = new(0.5f * Mathf.Sqrt(3.0f), 0.5f);
    public static readonly Vector2 BASE_Y = new(0.0f, 1.0f);
    public static readonly Vector2Int UP = new(0, 1);
    public static readonly Vector2Int UPLEFT = new(-1, 1);
    public static readonly Vector2Int UPRIGHT = new(1, 0);
    public static readonly Vector2Int DOWN = new(0, -1);
    public static readonly Vector2Int DOWNLEFT = new(-1, 0);
    public static readonly Vector2Int DOWNRIGHT = new(1, -1);
    private static readonly Dictionary<string, Layer> layers = new();

    void Update()
    {
        RainRandom();
    }

    void RainRandom()
    {
        Bounds mapBounds = MapManager.GetMapBounds();
        Vector2Int pos = GetClosestDataPoint(new Vector2(UnityEngine.Random.Range(mapBounds.min.x, mapBounds.max.x), UnityEngine.Random.Range(mapBounds.min.y, mapBounds.max.y)));
        SpatialData.SetValue("Water", pos, true);
    }
    public static void Initialize()
    {
        SpatialData.Clear();
        foreach (Layer layer in layers.Values)
        {
            Destroy(layer.gameObject);
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

    public class SpatialData
    {
        private static readonly Dictionary<Vector2Int, SpatialData> datapoints = new();
        public readonly Vector2Int hivePos;
        private readonly Dictionary<string, bool> data = new();

        private SpatialData(Vector2Int hivePos)
        {
            this.hivePos = hivePos;
            datapoints.Add(hivePos, this);
        }

        public void SetValue(string layerName, bool value)
        {
            if (!layers.ContainsKey(layerName))
            {
                GameObject layerObj = new GameObject(layerName + "Layer");
                layerObj.transform.position = Vector3.forward * TERRAIN_DEPTH;
                layerObj.AddComponent<MeshRenderer>();
                layerObj.AddComponent<MeshFilter>(); ;
                layerObj.AddComponent<PolygonCollider2D>();
                layers.Add(layerName, layerObj.AddComponent<Layer>());
            }
            bool oldValue;
            if (data.ContainsKey(layerName))
            {
                oldValue = data[layerName];
                data[layerName] = value;
            }
            else
            {
                oldValue = false;
                data.Add(layerName, value);
            }
            if (!oldValue && value)
                layers[layerName].AddShape(hivePos);
            if (oldValue && !value)
                layers[layerName].RemoveShape(hivePos);
        }
        public bool GetValue(string layerName)
        {
            return data.ContainsKey(layerName) ? data[layerName] : false;
        }

        public static void SetValue(string layerName, Vector2Int hivePos, bool value)
        {
            if (!IsDataPoint(hivePos)) throw new ArgumentException("Cannot set value on a vertex.");
            SpatialData datapoint = (datapoints.ContainsKey(hivePos)) ? datapoints[hivePos] : new SpatialData(hivePos);
            datapoint.SetValue(layerName, value);
        }

        public static bool GetValue(string layerName, Vector2Int hivePos)
        {
            if (!(IsDataPoint(hivePos) && datapoints.ContainsKey(hivePos))) return false;
            return datapoints[hivePos].GetValue(layerName);
        }

        public static IReadOnlyDictionary<Vector2Int, SpatialData> GetAllNonDefaultDatapoint() { return datapoints; }

        public static void Clear()
        {
            datapoints.Clear();
        }
    }

    private class Layer : MonoBehaviour
    {
        private string layerName;
        private readonly List<Vector2Int> hiveCoordinate = new();

        public void SetLayerName(string value) { layerName = value; }

        void Start()
        {
            GetComponent<MeshFilter>().mesh = new();
            GetComponent<PolygonCollider2D>().pathCount = 0;
        }

        void Update()
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Mesh newMesh = GetComponent<PolygonCollider2D>().CreateMesh(false, false);
            mesh.Clear();
            mesh.vertices = newMesh.vertices;
            mesh.triangles = newMesh.triangles;
            Destroy(newMesh);
        }

        public void AddShape(Vector2Int hivePos)
        {
            if (hiveCoordinate.Contains(hivePos)) return;
            PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
            hiveCoordinate.Add(hivePos);
            collider.pathCount++;
            collider.SetPath(collider.pathCount - 1, new Vector2[] {
                            Hive2Cartesian(hivePos + UP),
                            Hive2Cartesian(hivePos + UPLEFT),
                            Hive2Cartesian(hivePos + DOWNLEFT),
                            Hive2Cartesian(hivePos + DOWN),
                            Hive2Cartesian(hivePos + DOWNRIGHT),
                            Hive2Cartesian(hivePos + UPRIGHT)
                    });
        }

        public void RemoveShape(Vector2Int hivePos)
        {
            if (!hiveCoordinate.Contains(hivePos)) return;
            PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
            int index = hiveCoordinate.IndexOf(hivePos);
            collider.SetPath(index, collider.GetPath(collider.pathCount - 1));
            collider.pathCount--;
            hiveCoordinate[index] = hiveCoordinate[^1];
            hiveCoordinate.RemoveAt(hiveCoordinate.Count - 1);
        }
    }
}
