using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameObject m_character;
    private static float m_timer;

    // Start is called before the first frame update
    void Start()
    {
        m_character = GameObject.FindWithTag("Character");
        UIManager.m_timerText = GameObject.FindWithTag("TimerText").GetComponent<TextMeshProUGUI>();
        UIManager.m_levelText = GameObject.FindWithTag("LevelText").GetComponent<TextMeshProUGUI>();
        UIManager.m_gameplayPanel = GameObject.FindWithTag("GameplayPanel");
        UIManager.m_shopPanel = GameObject.FindWithTag("ShopPanel");
        UIManager.m_start = GameObject.FindWithTag("Start");
        UIManager.m_completePanel = GameObject.FindWithTag("CompletePanel");
        UIManager.m_activeJ = GameObject.FindWithTag("ActiveJ");
        UIManager.m_activeK = GameObject.FindWithTag("ActiveK");
        UIManager.m_activeL = GameObject.FindWithTag("ActiveL");
        UIManager.m_manaBar = GameObject.FindWithTag("ManaBar");

        Camera.main.GetComponent<CameraFocus>().SetFocus(m_character);
        LevelManager.Reset();
        m_timer = float.PositiveInfinity;
        MapManager.Initialize(LevelManager.GetMapSize(), LevelManager.GetTile(), LevelManager.GetWatermark());
        Pause();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer -= Time.deltaTime;
        UIManager.UpdateTimerText();
        if (m_character.GetComponent<Character>().GetHealth() <= 0.0f)
        {
            GameOver();
            return;
        }
        if (m_timer <= 0)
        {
            OpenShop();
            LevelManager.MoveNext();
            LoadLevel();
        }
    }

    public static GameObject getCharacter() {
        return m_character;
    }

    private static void LoadLevel()
    {
        Clear();
        UIManager.UpdateLevelText(LevelManager.GetLevelName());
        m_timer = LevelManager.GetTimeLimit();
        MapManager.Initialize(LevelManager.GetMapSize(), LevelManager.GetTile(), LevelManager.GetWatermark());        
    }

    private static void GameOver()
    {
        LevelManager.Reset();
        LoadLevel();
    }

    private static void Clear()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Disposable"))
            Destroy(o);
        m_character.transform.position = Vector3.zero;
    }

    public static float getTimer() { return m_timer; }
    public static void Pause()
    {
        UIManager.m_gameplayPanel.SetActive(false);
        Time.timeScale = 0.0f;
    }
    public static void Continue()
    {
        UIManager.m_start.SetActive(false);
        UIManager.m_gameplayPanel.SetActive(true);
        LoadLevel();
        Time.timeScale = 1.0f;
    }
    public static void OpenShop()
    {
        Pause();
        UIManager.m_shopPanel.SetActive(true);
        UIManager.SetShopOptions(LevelManager.GetShopOptions());
    }

    public static void CloseShop()
    {
        UIManager.m_shopPanel.SetActive(false);
        Continue();
    }
}
