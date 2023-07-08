using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

public class SpatialManager : MonoBehaviour
{
    public static readonly float TERRAIN_DEPTH = 0.25f;
    private static Bounds mapBounds;

    void Start()
    {
        // Test
        InvokeRepeating("rain", 1.0f, 0.1f);
    }

    void rain()
    {
        SpatialEntity data;
        do
        {
            data = SpatialEntity.GetClosestDataPoint(new Vector2(UnityEngine.Random.Range(mapBounds.min.x, mapBounds.max.x), UnityEngine.Random.Range(mapBounds.min.y, mapBounds.max.y)));
        } while (false && data.GetValue<bool>("IsWet"));
        Destroy(DrawCircle("Raindrop", SpatialEntity.Hive2Cartesian(data.hivePos), SpatialEntity.UNIT_SIZE / 2.0f, Color.cyan), 0.5f);
        data.SetValue("IsWet", true);
    }
    public static void Initialize(Bounds bounds)
    {
        mapBounds = bounds;
        SpatialEntity.Clear();
    }

    private class SpatialEntity
    {
        public static readonly float UNIT_SIZE = 1.0f;
        public static readonly Vector2 BASE_X = new(0.5f * Mathf.Sqrt(3.0f), 0.5f);
        public static readonly Vector2 BASE_Y = new(0.0f, 1.0f);
        public static readonly Vector2Int UP = new(0, 1);
        public static readonly Vector2Int UPLEFT = new(-1, 1);
        public static readonly Vector2Int UPRIGHT = new(1, 0);
        public static readonly Vector2Int DOWN = new(0, -1);
        public static readonly Vector2Int DOWNLEFT = new(-1, 0);
        public static readonly Vector2Int DOWNRIGHT = new(1, -1);
        private static readonly Dictionary<Vector2Int, SpatialEntity> Instances = new();
        public readonly Vector2Int hivePos;
        private readonly Dictionary<string, object> data = new();

        private SpatialEntity(Vector2Int hivePos)
        {
            this.hivePos = hivePos;
            Instances.Add(hivePos, this);
            Debug.Log(Instances.Count());
        }

        public void SetValue(string layerName, object value)
        {
            if (data.ContainsKey(layerName))
                data[layerName] = value;
            else
                data.Add(layerName, value);
        }
        public T GetValue<T>(string layerName)
        {
            return data.ContainsKey(layerName) ? (T)data[layerName] : default;
        }

        public static bool IsDataPoint(Vector2Int hivePos)
        {
            return (hivePos.x - hivePos.y) % 3 == 0;
        }

        public static SpatialEntity GetInstance(Vector2Int hivePos)
        {
            return IsDataPoint(hivePos) ? Instances.ContainsKey(hivePos) ? Instances[hivePos] : new(hivePos) : null;
        }

        public static SpatialEntity GetClosestDataPoint(Vector2 cartesian)
        {
            float x = Vector3.Cross(cartesian, BASE_Y).z / Vector3.Cross(BASE_X, BASE_Y).z / UNIT_SIZE;
            float y = Vector3.Cross(BASE_X, cartesian).z / Vector3.Cross(BASE_X, BASE_Y).z / UNIT_SIZE;
            Vector2Int quotient = new(Mathf.FloorToInt(x / 3.0f), Mathf.FloorToInt(y / 3.0f));
            Vector2 reminder = new Vector2(x, y) - quotient * 3;
            if (reminder.x < 1.0f || reminder.y >= 2.0f)
                return GetInstance(quotient * 3 + new Vector2Int(0, 3));
            if (reminder.x >= 2.0f || reminder.y < 1.0f)
                return GetInstance(quotient * 3 + new Vector2Int(3, 0));
            return (reminder.x + reminder.y) switch
            {
                < 1.0f => GetInstance(quotient * 3),
                < 3.0f => GetInstance((quotient * 3) + new Vector2Int(1, 1)),
                < 5.0f => GetInstance((quotient * 3) + new Vector2Int(2, 2)),
                _ => GetInstance((quotient * 3) + new Vector2Int(3, 3)),
            };
        }

        public static Vector2 Hive2Cartesian(Vector2 hivePos)
        {
            return hivePos.x * UNIT_SIZE * BASE_X + hivePos.y * UNIT_SIZE * BASE_Y;
        }

        public static void Clear()
        {
            Instances.Clear();
        }

        public static Vector2Int GetTangent(string boolLayerName, Vector2Int hivePos)
        {
            switch ((hivePos.x - hivePos.y) % 3)
            {
                case 0:
                    return Vector2Int.zero;
                case 1 or -2:
                    switch (
                        GetInstance(hivePos + UP).GetValue<bool>(boolLayerName),
                        GetInstance(hivePos + DOWNLEFT).GetValue<bool>(boolLayerName),
                        GetInstance(hivePos + DOWNRIGHT).GetValue<bool>(boolLayerName))
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
                        GetInstance(hivePos + DOWN).GetValue<bool>(boolLayerName),
                        GetInstance(hivePos + UPLEFT).GetValue<bool>(boolLayerName),
                        GetInstance(hivePos + UPRIGHT).GetValue<bool>(boolLayerName))
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

        public static List<List<Vector2>> GetContourLines(string boolLayerName, int interpolationDensity)
        {
            HashSet<Vector2Int> vertices = new();
            foreach (SpatialEntity entity in Instances.Values)
            {
                if (!entity.GetValue<bool>(boolLayerName))
                    continue;
                vertices.Add(entity.hivePos + UP);
            }

            List<List<Vector2>> paths = new();

            while (vertices.Count > 0)
            {
                Vector2Int startPos = RandomChoice(vertices); ;
                List<Vector2> smoothLine = new();
                Vector2Int prev = startPos;
                Vector2Int current = prev + GetTangent(boolLayerName, prev);
                Vector2Int next = current + GetTangent(boolLayerName, current);
                do
                {
                    vertices.Remove(current);
                    if (prev + next == current * 2)
                        smoothLine.Add((Hive2Cartesian(prev + current)) / 2.0f);
                    else
                    {
                        for (int j = 0; j < interpolationDensity; j++)
                        {
                            float t = (float)j / interpolationDensity;
                            smoothLine.Add(Hive2Cartesian(Vector2.Lerp(prev, current, t) + Vector2.Lerp(current, next, t)) / 2.0f);
                        }
                    }
                    prev = current;
                    current = next;
                    next = current + GetTangent(boolLayerName, current);
                } while (prev != startPos);
                if (smoothLine.Count > interpolationDensity)
                    paths.Add(smoothLine);
            }
            return paths;
        }
    }
}
