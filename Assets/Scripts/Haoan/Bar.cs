using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] GameObject m_target;
    [SerializeField] RectTransform m_rectTransform;
    [SerializeField] RectTransform m_parentRectTransform;
    [SerializeField] float m_offset;
    [SerializeField] float m_maxWidth;
    [SerializeField] Usage m_usage;

    enum Usage
    {
        Health,
        Mana
    }
    // Start is called before the first frame update
    void Start()
    {
        m_parentRectTransform = m_rectTransform.parent.GetComponent<RectTransform>();
        m_maxWidth = m_parentRectTransform.rect.width - m_offset;
    }

    // Update is called once per frame
    void Update()
    {
        float currentValue = 0.0f;
        float maxValue = 100.0f;

        switch(m_usage)
        {
            case Usage.Health:
                currentValue = m_target.GetComponent<Character>().GetHealth();
                maxValue = m_target.GetComponent<Character>().GetMaxHealth();
                break;
            case Usage.Mana:
                maxValue = 1.0f;
                currentValue = m_target.transform.GetChild(0).GetComponent<ActiveItem>().GetChargeProgress();
                break;
        }
        ShowBar(currentValue, maxValue);
    }

    void ShowBar(float currentValue, float maxValue)
    {
        float barTopValue = currentValue;
        barTopValue = Mathf.Clamp(barTopValue, 0.0f, maxValue);
        float width = barTopValue / maxValue * m_maxWidth;
        m_rectTransform.sizeDelta = new Vector2(width, m_rectTransform.sizeDelta.y);
    }

    public void SetTarget(GameObject target) { m_target = target;}
}
