using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] GameObject m_image;
    [SerializeField] GameObject m_mask;
    [SerializeField] GameObject m_frame;
    [SerializeField] GameObject m_text;
    [SerializeField] GameObject m_key;
    [SerializeField] GameObject m_chargeCount;

    public void SetLogo(Sprite sprite)
    {
        m_image.GetComponent<Image>().sprite = sprite;
    }

    public void SetSpinner(float value)
    {
        m_mask.GetComponent<Image>().fillAmount = value;
    }

    public void SetFrameActive(bool value)
    {
        m_frame.SetActive(value);
    }

    public void SetText(string text)
    {
        m_text.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetKey(KeyCode keyCode)
    {
        m_key.GetComponent<TextMeshProUGUI>().text = keyCode.ToString();
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
}
