using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private Shape m_shape;
    [SerializeField] private Sprite[] m_spriteArray;
    [SerializeField] private Color m_mapColor = new Color(0.3584906f, 0.3315237f, 0.2113742f);
    private SpriteRenderer m_spriteRenderer;

    enum Shape { 
        Rectangle,
        Circle,
        Triangle
    };
    void Start()
    {
        int index = Random.Range(0, m_spriteArray.Length-1);
        //int index = 1;
        Shape randomShape = (Shape)index;
        CreateMap(randomShape);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMap(Shape shape)
    {
        m_shape = shape;
        m_spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        m_spriteRenderer.color = m_mapColor;
        transform.position = new Vector3(0.0f,0.0f,0.5f);
        switch (shape)
        {
            case Shape.Rectangle:                
                m_spriteRenderer.sprite = m_spriteArray[0];
                transform.localScale = new Vector3(120.0f, 60.0f, 1.0f);
                break;
            case Shape.Circle:
                m_spriteRenderer.sprite = m_spriteArray[1];
                transform.localScale = new Vector3(80.0f, 80.0f, 1.0f);
                break;
            case Shape.Triangle:
                break;
        }
    }
    /// <summary>
    /// This function will take in a position and an offset with default value of 0.0f and return true if it's within a boundary or return false if it's out of a boundary.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="offset"></param>
    /// <returns>Returns the result.</returns>
    public bool IsInMap(Vector3 pos, float offset = 0.0f)
    {
        bool returnVal = false;
        switch (m_shape)
        {
            case Shape.Rectangle:
                Vector3 min = transform.position - transform.localScale / 2;
                Vector3 max = transform.position + transform.localScale / 2;
                if (transform.position.x < min.x + offset || transform.position.x > max.x - offset || transform.position.y < min.y + offset || transform.position.y > max.y - offset)
                    returnVal = false;
                else
                {
                    returnVal = true;
                }
                break;
            case Shape.Circle:
                float radius = transform.localScale.x - offset;
                float distance = Vector3.Distance(transform.position, pos);
                if (distance <= radius)
                {
                    returnVal = true;
                }
                else
                {
                    returnVal = false;
                }
                break;
            case Shape.Triangle:
                break;
        }
        return returnVal;
    }
}
