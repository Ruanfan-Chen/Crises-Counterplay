using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Supercharge : MonoBehaviour
{
    [Header("UI items for Spell Cooldown")]
    [SerializeField] private Image m_imageCooldown;
    [SerializeField] private TMP_Text m_textCooldown;
    [SerializeField] private Image m_imageEdge;

    [Space(10)]
    [Header("Player and Character")]
    [SerializeField] private ActiveItem m_activeItem;

    private float m_cooldownTime;
    private Image m_icon;

    public void SetActiveItem(ActiveItem activeItem) { m_activeItem = activeItem; }
    // Start is called before the first frame update
    void Start()
    {
        m_textCooldown.gameObject.SetActive(true);
        m_imageEdge.gameObject.SetActive(false);
        m_imageCooldown.fillAmount = 0.0f;

        m_icon = GetComponent<Image>();
        m_icon.sprite = m_activeItem.GetLogo();
    }

    // Update is called once per frame
    void Update()
    {
        m_textCooldown.text = Mathf.Round(m_activeItem.GetChargeProgress() * 5.0f).ToString();
        if (m_activeItem.IsUsable())
        {
            m_imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            m_imageCooldown.fillAmount = 1.0f;
        }
    }
}
