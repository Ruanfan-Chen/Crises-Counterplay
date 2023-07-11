using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    [Header("UI items for Spell Cooldown")]
    [SerializeField] private Image m_imageCooldown;
    [SerializeField] private Image m_imageEdge;

    [Space(10)]
    [Header("Player and Character")]
    [SerializeField] private ActiveItem m_activeItem;

    private float m_cooldownTime;
    private Image m_icon;

    public void SetActiveItem(ActiveItem activeItem, Sprite logo)
    {
        m_activeItem = activeItem;
        m_icon = GetComponent<Image>();
        m_icon.sprite = logo;
    }
    public void SetCooldownTime(float cd) { m_cooldownTime = cd; }
    // Start is called before the first frame update
    void Start()
    {
        m_imageEdge.gameObject.SetActive(false);
        m_imageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = m_activeItem.GetChargeProgress();
        if (m_activeItem.IsUsable())
        {
            m_imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            if (progress == 1.0f)
            {
                m_imageCooldown.fillAmount = 1.0f;
            }
            else
            {
                m_imageCooldown.fillAmount = 1.0f - progress;
            }
        }

        if (m_imageCooldown.fillAmount == 0.0f)
        {
            m_imageEdge.gameObject.SetActive(false);
        }
        else
        {
            if (progress != 1.0f)
            {
                m_imageEdge.gameObject.SetActive(true);
                m_imageEdge.transform.localEulerAngles = new Vector3(0, 0, 360.0f * (1.0f - progress));
            }
            else
            {
                m_imageEdge.gameObject.SetActive(false);
            }
        }
    }
}
