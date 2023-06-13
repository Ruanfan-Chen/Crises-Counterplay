using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] GameObject m_character;
    [SerializeField] RectTransform m_rectTransform;
    [SerializeField] RectTransform m_parentRectTransform;
    [SerializeField] float m_offset;
    [SerializeField] float m_maxWidth;

    // Start is called before the first frame update
    void Start()
    {
        m_parentRectTransform = m_rectTransform.parent.GetComponent<RectTransform>();
        m_maxWidth = m_parentRectTransform.rect.width - m_offset;
    }

    // Update is called once per frame
    void Update()
    {
        float hp = m_character.GetComponent<Character>().GetHealth();
        hp = Mathf.Clamp(hp, 0.0f, m_character.GetComponent<Character>().GetMaxHealth());
        float width = hp / m_character.GetComponent<Character>().GetMaxHealth() * m_maxWidth;
        m_rectTransform.sizeDelta = new Vector2(width, m_rectTransform.sizeDelta.y);

        GameObject canvas = m_parentRectTransform.parent.gameObject;
        float z = 0.0f - m_character.transform.localEulerAngles.z;
        canvas.transform.localEulerAngles = new Vector3(0.0f, 0.0f, z);
    }
}
