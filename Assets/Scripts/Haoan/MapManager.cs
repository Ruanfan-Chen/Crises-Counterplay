using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int m_widthNum;
    [SerializeField] private int m_heightNum;
    int m_gridSize = 16;
    int m_xLower;
    int m_yLower;
    int m_xUpper;
    int m_yUpper;
    Vector2 m_size;

    [SerializeField] GameObject m_floorPrefab;
    [SerializeField] private GameObject m_player;
    [SerializeField] private Shape m_shape;
    [SerializeField] private Sprite[] m_spriteArray;
    [SerializeField] private Color m_mapColor = new Color(0.3584906f, 0.3315237f, 0.2113742f);
    [SerializeField] private GameObject[] m_elementArray;
    private SpriteRenderer m_spriteRenderer;

    private int m_level;

    public enum Shape
    {
        Rectangle,
        Circle,
        Triangle
    };

    void Start()
    {
        // m_event_next_occurance = m_event_intervl_min + (float)((m_event_intervl_max - m_event_intervl_min) * rnd.NextDouble());
        m_xLower = -m_widthNum * m_gridSize;
        m_xUpper = m_widthNum * m_gridSize;
        m_yLower = -m_heightNum * m_gridSize;
        m_yUpper = m_heightNum * m_gridSize;
    }

    public void CreateMap(Shape shape)
    {
        m_shape = shape;
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (!m_spriteRenderer)
        {
            m_spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        }
        m_spriteRenderer.color = m_mapColor;
        transform.position = new Vector3(0.0f, 0.0f, 0.5f);
        switch (shape)
        {
            case Shape.Rectangle:
                m_spriteRenderer.sprite = m_spriteArray[0];
                transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
                m_size = new Vector2(60.7f, 30.4f);

                //create the map by tiling
                /*if(transform.childCount == 0)
                {
                    for (int y = -m_heightNum * m_gridSize; y < m_heightNum * m_gridSize; y += m_gridSize)
                    {
                        for (int x = -m_widthNum * m_gridSize; x < m_widthNum * m_gridSize; x += m_gridSize)
                        {
                            GameObject floor = Instantiate(m_floorPrefab);
                            floor.transform.SetParent(transform);
                            floor.transform.localPosition = new Vector3(x, y, 0);
                        }
                    }
                    m_size = new Vector2(m_gridSize * m_widthNum * 2, m_gridSize * m_heightNum * 2);
                }*/
                break;
            case Shape.Circle:
                break;
            case Shape.Triangle:
                break;
        }
    }

    public void LoadLevel(int levelNum)
    {
        m_level = levelNum;

        if (levelNum == 1)
        {
            CreateMap(Shape.Rectangle);
        }
        else if (levelNum == 2)
        {
            CreateMap(Shape.Rectangle);
        }
        else
        {
            LoadLevel(2);
        }
    }
    /// <summary>
    /// This function will take in a position and an offset with default value of 0.0f and return true if it's within a boundary or return false if it's out of a boundary.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="offset"></param>
    /// <returns>Returns the result.</returns>
    public Vector3 PosInMap(Vector3 pos, float offset = 0.0f)
    {
        Vector3 returnVal = pos;
        switch (m_shape)
        {
            case Shape.Rectangle:
                Vector3 min = transform.position - new Vector3(m_size.x, m_size.y, 1.0f) / 2;
                Vector3 max = transform.position + new Vector3(m_size.x, m_size.y, 1.0f) / 2;
                if (pos.x < min.x + offset)
                {
                    returnVal.x = min.x + offset;
                }
                else if (pos.x > max.x - offset)
                {
                    returnVal.x = max.x - offset;
                }
                if (pos.y < min.y + offset)
                {
                    returnVal.y = min.y + offset;
                }
                else if (pos.y > max.y - offset)
                {
                    returnVal.y = max.y - offset;
                }
                break;
            case Shape.Circle:
                float radius = transform.localScale.x / 2 - offset;
                Vector3 origin = transform.position;
                origin.z = 0.0f;
                float distance = Vector3.Distance(origin, pos);
                if (distance > radius)
                {
                    float angle = Mathf.Atan2(pos.y, pos.x);
                    returnVal.x = radius * Mathf.Cos(angle);
                    returnVal.y = radius * Mathf.Sin(angle);
                }
                break;
            case Shape.Triangle:
                break;
        }
        return returnVal;
    }

    public bool IsInMap(Vector3 pos, float offset = 0.0f)
    {
        return pos == PosInMap(pos, offset);
    }

    public Vector2 GetMapScale()
    {
        return m_size;
    }

    public Vector3 GetRandomPointOnEdge()
    {
        Vector2 mapScale = GetMapScale();
        float r = Random.Range(0.0f, (mapScale.x + mapScale.y) * 2);
        if (r < mapScale.x)
            return new Vector3(Random.Range(-mapScale.x / 2, mapScale.x / 2), -mapScale.y / 2);
        else if (r < mapScale.x + mapScale.y)
            return new Vector3(-mapScale.x / 2, Random.Range(-mapScale.y / 2, mapScale.y / 2));
        else if (r < mapScale.x * 2 + mapScale.y)
            return new Vector3(Random.Range(-mapScale.x / 2, mapScale.x / 2), mapScale.y / 2);
        else
            return new Vector3(mapScale.x / 2, Random.Range(-mapScale.y / 2, mapScale.y / 2));
    }
}
