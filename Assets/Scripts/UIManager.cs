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
                GameplayManager.GetGoogleSender().SendMatrix4(option.GetName());
                GameplayManager.CloseShop();
            });
            GameObject label = new GameObject(option.GetName() + "Label");
            label.transform.SetParent(optionObj.transform);
            label.AddComponent<RectTransform>();
            label.GetComponent<RectTransform>().localPosition = Vector3.down * 30.0f;
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
            UIScript.SetFrameActive(item.IsUsable());
            UIScript.SetKey(keyCode);
            UIScript.SetText("");
            float chargeProgress = item.GetChargeProgress();
            UIScript.SetSpinner(Mathf.Ceil(chargeProgress) - chargeProgress);
            UIScript.SetChargeCount(Mathf.FloorToInt(chargeProgress));
        }
    }
}