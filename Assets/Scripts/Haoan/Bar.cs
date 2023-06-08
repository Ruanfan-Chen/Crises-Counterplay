using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] GameObject m_character;
    [SerializeField] RectTransform m_rectTransform;
    [SerializeField] float m_offset;
    [SerializeField] float m_maxWidth;

    CharacterAttribute m_attribute;
    // Start is called before the first frame update
    void Start()
    {
        m_attribute = m_character.GetComponent<CharacterAttribute>();
        m_maxWidth = m_rectTransform.parent.GetComponent<RectTransform>().rect.width-m_offset;
    }

    // Update is called once per frame
    void Update()
    {
        float width = m_attribute.GetHealth() / m_attribute.GetMaxHealth() * m_maxWidth;
        m_rectTransform.sizeDelta = new Vector2(width, m_rectTransform.sizeDelta.y);
    }
}
