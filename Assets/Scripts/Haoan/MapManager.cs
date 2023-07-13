using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int m_widthNum;
    [SerializeField] private int m_heightNum;
    [SerializeField] GameObject m_wasd;
    [SerializeField] GameObject m_tilePrefab;

    [Space(10)]
    [Header("Sprites")]
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private Sprite m_trainboundSprite;
    [SerializeField] private Sprite m_chistrikeSprite;
    [SerializeField] private Sprite m_superchargeSprite;
    private SpriteRenderer m_spriteRenderer;

    public static readonly float MAP_DEPTH = 0.5f;
    private int m_level;
    static Bounds m_bounds;

    public void CreateMap()
    {
        CreateMapBySprite(m_sprite);
    }

    public void CreateMapBySprite(Sprite sprite)
    {
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (!m_spriteRenderer)
        {
            m_spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        transform.position = new Vector3(0.0f, 0.0f, MAP_DEPTH);
        m_spriteRenderer.sprite = sprite;
        transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
        m_bounds = new Bounds(Vector3.zero, new Vector3(60.7f, 52.8f));
    }

    public void CreateMapByTile()
    {
        for (int x = -m_widthNum; x <= m_widthNum; x++)
        {
            for (int y = -m_heightNum; y <= m_heightNum; y++)
            {
                Vector3 tilePosition = new Vector3(x, y, MAP_DEPTH);
                GameObject newTile = Instantiate(m_tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = transform;
            }
        }
        Vector3 wasdPosition = new Vector3(0f, 0f, MAP_DEPTH);
        GameObject wasd = Instantiate(m_wasd, wasdPosition, Quaternion.identity);
        m_bounds = new Bounds(Vector3.zero, new Vector3(2 * m_widthNum + 1, 2 * m_heightNum + 1));
    }

    public void LoadLevel(int levelNum)
    {
        switch(levelNum)
        {
            case 1:
                CreateMapBySprite(m_sprite);
                break;
            case 2:
                CreateMapBySprite(m_trainboundSprite);
                break;
            case 110:
                CreateMapBySprite(m_trainboundSprite);
                break;
            case 111:
                CreateMapBySprite(m_chistrikeSprite);
                break;
            case 210:
                CreateMapBySprite(m_superchargeSprite);
                break;
        }
    }

    /// <summary>
    /// This function will take in a position and an offset with default value of 0.0f and return true if it's within a boundary or return false if it's out of a boundary.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="offset"></param>
    /// <returns>Returns the result.</returns>
    static public Vector3 PosInMap(Vector3 pos, float offset = 0.0f)
    {
        Bounds bounds = GetMapBounds(offset);
        return new Vector3(Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x), Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y), pos.z);
    }

    static public bool IsInMap(Vector3 pos, float offset = 0.0f)
    {
        return pos == PosInMap(pos, offset);
    }

    static public Bounds GetMapBounds(float offset = 0.0f)
    {
        return new Bounds(m_bounds.center, m_bounds.size - 2 * new Vector3(offset, offset));
    }

    static public Vector3 GetRandomPointOnEdge(float offset = 0.0f)
    {
        Bounds bounds = GetMapBounds(offset);
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
            0 => new Vector3(randX, bounds.min.y),
            1 => new Vector3(bounds.min.x, randY),
            2 => new Vector3(randX, bounds.max.y),
            3 => new Vector3(bounds.max.x, randY),
            _ => default,
        };
    }
}
