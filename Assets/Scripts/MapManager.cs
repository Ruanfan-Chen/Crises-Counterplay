using System;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public static class MapManager
{
    private static string wasdSpritePath = "Sprites/wasd";
    public static readonly float MAP_DEPTH = 0.5f;
    public static readonly float WATERMARK_DEPTH = 0.4f;
    private static Bounds bounds;
    private static GameObject map;

    public static void Initialize(Vector2 size, Sprite tile)
    {
        if (map)
            UnityEngine.Object.Destroy(map);
        bounds = new Bounds(Vector3.zero, size);

        map = new GameObject("Map");
        map.transform.position = Vector3.forward * MAP_DEPTH;

        SpriteRenderer mapRenderer = map.AddComponent<SpriteRenderer>();
        mapRenderer.sprite = tile;
        mapRenderer.drawMode = SpriteDrawMode.Tiled;
        mapRenderer.size = size;

        GameObject wasdObj = new GameObject("wasd");
        wasdObj.transform.SetParent(map.transform);
        wasdObj.transform.localPosition = Vector3.forward * (WATERMARK_DEPTH - MAP_DEPTH);
        wasdObj.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
        wasdObj.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(wasdSpritePath);
    }

    static public Bounds GetBounds(float offset = 0.0f)
    {
        return new Bounds(bounds.center, bounds.size - 2 * new Vector3(offset, offset));
    }

    static public Vector2 GetRandomPointOnEdge(float offset = 0.0f)
    {
        Bounds bounds = GetBounds(offset);
        float randX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        return WeightedRandom(new Dictionary<int, float>()
        {
            [0] = bounds.size.x,
            [1] = bounds.size.y,
            [2] = bounds.size.x,
            [3] = bounds.size.y
        }) switch
        {
            0 => new Vector2(randX, bounds.min.y),
            1 => new Vector2(bounds.min.x, randY),
            2 => new Vector2(randX, bounds.max.y),
            3 => new Vector2(bounds.max.x, randY),
            _ => default,
        };
    }

    static public Vector2 GetRandomPointInMap(float offset = 0.0f)
    {
        Bounds bounds = GetBounds(offset);
        return new(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y));
    }
}
