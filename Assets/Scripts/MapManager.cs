using System.Collections.Generic;
using UnityEngine;
using static Utility;

public static class MapManager
{
    public static readonly float MAP_DEPTH = 0.5f;
    private static Bounds bounds;
    private static GameObject map;

    public static void Initialize(Vector2 size, Sprite tile, Sprite watermark)
    {
        if (map)
            Object.Destroy(map);
        bounds = new Bounds(Vector3.zero, size);

        map = new GameObject("Map");
        map.transform.position = Vector3.forward * MAP_DEPTH;

        SpriteRenderer mapRenderer = map.AddComponent<SpriteRenderer>();
        mapRenderer.sprite = tile;
        mapRenderer.drawMode = SpriteDrawMode.Tiled;
        mapRenderer.size = size;

        GameObject watermarkObj = new GameObject("Watermark");
        watermarkObj.transform.SetParent(map.transform, false);

        SpriteRenderer watermarkRenderer = watermarkObj.AddComponent<SpriteRenderer>();
        watermarkRenderer.sprite = watermark;
        watermarkRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }

    static public Bounds GetBounds(float offset = 0.0f)
    {
        return new Bounds(bounds.center, bounds.size - 2 * new Vector3(offset, offset));
    }

    static public Vector2 GetRandomPointOnEdge(float offset = 0.0f)
    {
        Bounds bounds = GetBounds(offset);
        float randX = Random.Range(bounds.min.x, bounds.max.x);
        float randY = Random.Range(bounds.min.y, bounds.max.y);
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
        return new(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
    }
}
