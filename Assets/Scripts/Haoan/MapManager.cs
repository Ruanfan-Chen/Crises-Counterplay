using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private Shape m_shape;
    [SerializeField] private Sprite[] m_spriteArray;
    [SerializeField] private Color m_mapColor = new Color(0.3584906f, 0.3315237f, 0.2113742f);
    [SerializeField] private GameObject[] m_elementArray;
    private SpriteRenderer m_spriteRenderer;

    private float m_timer;

    // event will occur within [m_event_intervl_min, m_event_intervl_max] after last occurance
    public float m_event_intervl_min = 5.0f; // min time (in seconds) after last event occurred

    public float m_event_intervl_max = 9.0f; // max time (in seconds) after last event occurred

    private float m_event_next_occurance;

    private int m_level;

    private System.Random rnd = new System.Random();

    public enum Shape
    {
        Rectangle,
        Circle,
        Triangle
    };

    void Start()
    {
        // m_event_next_occurance = m_event_intervl_min + (float)((m_event_intervl_max - m_event_intervl_min) * rnd.NextDouble());
    }

    // Update is called once per frame
    void Update()
    {
        checkAndGenerateEvent(m_timer);
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

    public void LoadLevel(int levelNum)
    {
        m_level = levelNum;
        // Update event interval; m_level >= 1
        m_event_intervl_max = Math.Max(1.0f, m_event_intervl_max - (m_level - 1.0f));
        m_event_intervl_min = Math.Max(1.0f, m_event_intervl_min - (m_level - 1.0f));
        Debug.Log("m_event_intervl_min = " + m_event_intervl_min + ", m_event_intervl_max = " + m_event_intervl_max);
        initEventsGenerator();
        if (levelNum == 1)
        {
            CreateMap(Shape.Rectangle);
            GameObject lava = Instantiate(m_elementArray[0]);
            lava.transform.position = new Vector3(0.0f, 27.5f, 0.0f);
            lava.transform.localScale = new Vector3(120.0f, 5.0f, 1.0f);

        }
        else if (levelNum == 2)
        {
            CreateMap(Shape.Rectangle);
            GameObject lava = Instantiate(m_elementArray[0]);
            lava.transform.position = new Vector3(0.0f, 27.5f, 0.0f);
            lava.transform.localScale = new Vector3(120.0f, 5.0f, 1.0f);
            GameObject spring = Instantiate(m_elementArray[1]);
            spring.transform.position = new Vector3(0.0f, -27.5f, 0.0f);
            spring.transform.localScale = new Vector3(120.0f, 5.0f, 1.0f);
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
                Vector3 min = transform.position - transform.localScale / 2;
                Vector3 max = transform.position + transform.localScale / 2;
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

    public void setTimer(float t)
    {
        m_timer = t;
    }

    private void checkAndGenerateEvent(float t)
    {
        if (Math.Abs(t - m_event_next_occurance) < 0.1)
        {
            // trigger event
            Vehicle.Instantiate();
            Debug.Log("!!! event occurs at t = " + t);
            // update next occurace
            m_event_next_occurance = t + m_event_intervl_min + (float)((m_event_intervl_max - m_event_intervl_min) * rnd.NextDouble());
            Debug.Log("m_event_next_occurance = " + m_event_next_occurance);

        }
    }

    public void initEventsGenerator()
    {
        m_event_next_occurance = m_event_intervl_min + (float)((m_event_intervl_max - m_event_intervl_min) * rnd.NextDouble());
        Debug.Log("m_event_next_occurance = " + m_event_next_occurance);
    }
}
