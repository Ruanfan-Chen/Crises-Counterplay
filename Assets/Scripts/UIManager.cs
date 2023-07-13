using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    public static TextMeshProUGUI m_timerText;
    public static TextMeshProUGUI m_levelText;
    public static GameObject m_gameplayPanel;
    public static GameObject m_shopPanel;
    public static GameObject m_start;
    public static GameObject m_completePanel;
    public static GameObject m_activeJ;
    public static GameObject m_activeK;
    public static GameObject m_activeL;
    public static GameObject m_manaBar;

    public static void UpdateLevelText(string levelName)
    {
        m_levelText.text = levelName;
    }

    public static void UpdateTimerText()
    {
        m_timerText.text = Mathf.Round(GameplayManager.getTimer()).ToString() + "s";
    }

    public static void ClearShopPanel()
    {
        for (int i = 0; i < m_shopPanel.transform.childCount; i++)
            Object.Destroy(m_shopPanel.transform.GetChild(i).gameObject);
    }

    public static void SetShopOptions(IReadOnlyList<LevelManager.ShopOption> shopOptions)
    {
        for (int i = 0; i < shopOptions.Count; i++)
        {
            LevelManager.ShopOption option = shopOptions[i];
            float xRatio = (float)(i + 1) / (shopOptions.Count + 1);
            GameObject optionObj = new GameObject(option.GetName());
            optionObj.transform.SetParent(m_shopPanel.transform);
            optionObj.AddComponent<RectTransform>();
            optionObj.GetComponent<RectTransform>().anchorMin = new Vector2(xRatio, 0.5f);
            optionObj.GetComponent<RectTransform>().anchorMax = new Vector2(xRatio, 0.5f);
            optionObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            optionObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100.0f);
            optionObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100.0f);
            optionObj.AddComponent<Image>().sprite = option.GetLogo();
            optionObj.AddComponent<Button>().onClick.AddListener(() =>
            {
                option.GetAction()();
                ClearShopPanel();
                GameplayManager.CloseShop();
            });
            GameObject label = new GameObject(option.GetName() + "Label");
            label.transform.SetParent(optionObj.transform);
            label.AddComponent<RectTransform>();
            label.GetComponent<RectTransform>().localPosition = Vector3.down * 30.0f;
        }
    }
}