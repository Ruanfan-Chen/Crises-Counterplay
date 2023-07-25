using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    public static readonly string ActiveSkillPrefabPath = "Prefabs/ActiveSkill";
    public static TextMeshProUGUI m_timerText;
    public static TextMeshProUGUI m_currentRecordText;
    public static TextMeshProUGUI m_highestRecordText;
    public static TextMeshProUGUI m_beatPctText;
    public static TextMeshProUGUI m_currBestTextLS;
    public static TextMeshProUGUI m_highestRecordTextLS;
    public static TextMeshProUGUI m_currBestLabelGP;
    public static TextMeshProUGUI m_highestRecordLabelGP;
    public static TextMeshProUGUI m_currBestTextGP;
    public static TextMeshProUGUI m_highestRecordTextGP;
    public static GameObject m_gameplayPanel;
    public static GameObject m_shopPanel;
    public static GameObject m_completePanel;
    public static GameObject m_activeSkillPanel;
    public static GameObject m_levelSelectionPanel;
    public static GameObject m_losePanel;
    public static GameObject m_infiniteModePanel;
    public static GameObject m_recordsPanel;


    public static void UpdateTimerText()
    {
        m_timerText.text = Mathf.Round(GameplayManager.GetTimer()).ToString() + "s";
    }

    public static void UpdateScoresText(float currentScore, double beatPct, int precision)
    {
        m_currentRecordText.text = RoundDecimal(currentScore, precision).ToString() + "s";
        m_highestRecordText.text = RoundDecimal(GameplayManager.GetHighestRecord(), precision).ToString() + "s";
        Debug.Log("beatPct = " + beatPct);
        m_beatPctText.text = System.Math.Round(beatPct * 100, 2).ToString("0.00") + "%";
    }

    public static void ClearShopPanel()
    {
        for (int i = 0; i < m_shopPanel.transform.childCount; i++)
            Object.Destroy(m_shopPanel.transform.GetChild(i).gameObject);
    }

    public static void SetShopOptions(IReadOnlyList<LevelManager.OptionConfig> optionConfigs)
    {
        for (int i = 0; i < optionConfigs.Count; i++)
        {
            GameObject option = optionConfigs[i].Instantiate();
            float xAnchor = (i + 1.0f) / (optionConfigs.Count + 1);
            option.GetComponent<RectTransform>().SetParent(m_shopPanel.transform, false);
            option.GetComponent<RectTransform>().anchorMin = new Vector2(xAnchor, 0.5f);
            option.GetComponent<RectTransform>().anchorMax = new Vector2(xAnchor, 0.5f);
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
            UIScript.SetSpinner(chargeProgress > 0.0f ? Mathf.Ceil(chargeProgress) - chargeProgress : 1.0f);
            UIScript.SetChargeCount(Mathf.FloorToInt(chargeProgress));
        }
    }


    public static void UpdateRecordsLS(float currBest, float highestRecord)
    {
        Debug.Log("UpdateRecordsLS()");
        m_currBestTextLS.text = RoundDecimal(currBest, 1).ToString() + "s";
        m_highestRecordTextLS.text = RoundDecimal(highestRecord, 1).ToString() + "s";
    }

    public static void ControlRecordsGP(float currBest, float highestRecord, bool show)
    {
        Debug.Log("ControlRecordsGP()");
        if (show)
        {
            m_currBestLabelGP.text = "Current best:";
            m_highestRecordLabelGP.text = "Highest record:";
            m_currBestTextGP.text = RoundDecimal(currBest, 1).ToString() + "s";
            m_highestRecordTextGP.text = RoundDecimal(highestRecord, 1).ToString() + "s";

        }
        else
        {
            m_currBestLabelGP.text = "";
            m_highestRecordLabelGP.text = "";
            m_currBestTextGP.text = "";
            m_highestRecordTextGP.text = "";

        }

    }

    public static double RoundDecimal(float num, int precision)
    {
        double tmp = System.Math.Pow(10, precision);
        return System.Math.Truncate(num * tmp) / tmp;


    }
}