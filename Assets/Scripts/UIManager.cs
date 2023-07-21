using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    public static readonly string ActiveSkillPrefabPath = "Prefabs/ActiveSkill";
    public static TextMeshProUGUI m_timerText;
    public static GameObject m_gameplayPanel;
    public static GameObject m_shopPanel;
    public static GameObject m_completePanel;
    public static GameObject m_activeSkillPanel;
    public static GameObject m_levelSelectionPanel;
    public static GameObject m_losePanel;

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
            float xAnchor = (i + 1.0f) / (shopOptions.Count + 1);

            GameObject logo = new(option.GetName() + "Logo");
            logo.AddComponent<RectTransform>();
            logo.GetComponent<RectTransform>().parent = m_shopPanel.GetComponent<RectTransform>();
            logo.GetComponent<RectTransform>().anchorMin = new(xAnchor, 0.8f);
            logo.GetComponent<RectTransform>().anchorMax = new(xAnchor, 0.8f);
            logo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            logo.AddComponent<Image>().sprite = option.GetLogo();
            logo.AddComponent<Button>().onClick.AddListener(() =>
            {
                option.GetAction()();
                ClearShopPanel();
                GameplayManager.GetGoogleSender().SendMatrix4(option.GetName());
                GameplayManager.CloseShop();
            });


            GameObject label = new(option.GetName() + "Label");
            label.AddComponent<RectTransform>();
            label.GetComponent<RectTransform>().parent = m_shopPanel.GetComponent<RectTransform>();
            label.GetComponent<RectTransform>().anchorMin = new(xAnchor, 0.6f);
            label.GetComponent<RectTransform>().anchorMax = new(xAnchor, 0.6f);
            label.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            label.AddComponent<TextMeshProUGUI>().text = option.GetName();


            GameObject tutorial = new(option.GetName() + "Tutorial");
            tutorial.AddComponent<RectTransform>();
            tutorial.GetComponent<RectTransform>().parent = m_shopPanel.GetComponent<RectTransform>();
            tutorial.GetComponent<RectTransform>().anchorMin = new(xAnchor, 0.4f);
            tutorial.GetComponent<RectTransform>().anchorMax = new(xAnchor, 0.4f);
            tutorial.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Sprite sprite = option.GetTutorial();
            tutorial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 235.0f);
            tutorial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 125.0f);
            tutorial.AddComponent<Image>().sprite = sprite;


            GameObject description = new(option.GetName() + "Description");
            description.AddComponent<RectTransform>();
            description.GetComponent<RectTransform>().parent = m_shopPanel.GetComponent<RectTransform>();
            description.GetComponent<RectTransform>().anchorMin = new(xAnchor, 0.2f);
            description.GetComponent<RectTransform>().anchorMax = new(xAnchor, 0.2f);
            description.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //description.AddComponent<TextMeshProUGUI>().text = option.GetDescription();
        }
    }

    public static void UpdateActiveSkills(IReadOnlyDictionary<KeyCode, ActiveItem> readOnlyDictionary)
    {
        List<KeyCode> KeyCodes = readOnlyDictionary.Keys.ToList();
        KeyCodes.Sort();
        if (m_activeSkillPanel.transform.childCount != KeyCodes.Count)
        {
            for (int i = 0; i < m_activeSkillPanel.transform.childCount; i++)
                Object.Destroy(m_activeSkillPanel.transform.GetChild(i).gameObject);

            for (int i = 0; i < KeyCodes.Count; i++)
            {
                GameObject activeSkill = Object.Instantiate(Resources.Load<GameObject>(ActiveSkillPrefabPath), m_activeSkillPanel.transform);
                activeSkill.name = "ActiveSkill_" + KeyCodes[i];
                activeSkill.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.0f);
                activeSkill.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.0f);
                activeSkill.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 200.0f - KeyCodes.Count * 100.0f + 100.0f, 50.0f);
            }
        }
        for (int i = 0; i < KeyCodes.Count; i++)
        {
            KeyCode keyCode = KeyCodes[i];
            ActiveItem item = readOnlyDictionary[keyCode];
            ActiveSkill UIScript = m_activeSkillPanel.transform.GetChild(i).GetComponent<ActiveSkill>();
            UIScript.SetLogo(item.GetUISprite());
            UIScript.SetKey(keyCode);
            UIScript.SetText("");
            UIScript.SetUsable(item.IsUsable());
            float chargeProgress = item.GetChargeProgress();
            UIScript.SetSpinner(Mathf.Ceil(chargeProgress) - chargeProgress);
            UIScript.SetChargeCount(Mathf.FloorToInt(chargeProgress));
        }
    }
}