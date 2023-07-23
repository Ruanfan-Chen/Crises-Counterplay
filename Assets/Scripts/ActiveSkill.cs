using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] Image m_image;
    [SerializeField] Image m_mask;
    [SerializeField] TextMeshProUGUI m_text;
    [SerializeField] TextMeshProUGUI m_key;
    [SerializeField] GameObject m_chargeCount;
    [SerializeField] GameObject m_notUsable;

    public void SetLogo(Sprite sprite)
    {
        m_image.sprite = sprite;
    }

    public void SetSpinner(float value)
    {
        m_mask.fillAmount = value;
    }

    public void SetText(string text)
    {
        m_text.text = text;
    }

    public void SetKey(KeyCode keyCode)
    {
        m_key.text = keyCode.ToString();
    }

    public void SetChargeCount(int value)
    {
        if (value <= 1)
            m_chargeCount.SetActive(false);
        else
        {
            m_chargeCount.SetActive(true);
            m_chargeCount.GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }

    public void SetUsable(bool value)
    {
        m_notUsable.SetActive(!value);
    }
}
