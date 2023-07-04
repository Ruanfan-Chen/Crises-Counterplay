using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int m_widthNum;
    [SerializeField] private int m_heightNum;
    Vector2 m_size;
    [SerializeField] GameObject m_wasd;
    [SerializeField] GameObject m_tilePrefab;
    [SerializeField] private Sprite m_sprite;
    private SpriteRenderer m_spriteRenderer;

    private int m_level;


    void Start()
    {
    }

    public void CreateMap()
    {
        //m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //if (!m_spriteRenderer)
        //{
        //    m_spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        //}
        //transform.position = new Vector3(0.0f, 0.0f, 0.5f);
        //m_spriteRenderer.sprite = m_sprite;
        //transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
        //m_size = new Vector2(60.7f, 30.4f);
        CreateMapByTile();
    }

    public void CreateMapByTile()
    {
        for(int x = -m_widthNum; x <= m_widthNum; x++)
        {
            for (int y = -m_heightNum; y <= m_heightNum; y++)
            {
                Vector3 tilePosition = new Vector3(x, y, 0.5f);
                GameObject newTile = Instantiate(m_tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = transform;
            }
        }
        Vector3 wasdPosition = new Vector3(0f, 0f, 0.5f);
        GameObject wasd = Instantiate(m_wasd, wasdPosition, Quaternion.identity);
        m_size = new Vector2(2 * m_widthNum + 1, 2 * m_heightNum + 1);
    }

    public void LoadLevel(int levelNum)
    {
        m_level = levelNum;
        CreateMap();
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
