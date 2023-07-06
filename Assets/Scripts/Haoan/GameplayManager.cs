using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private GameObject m_gameplayPanel;
    [SerializeField] private GameObject m_shopPanel;
    [SerializeField] private GameObject m_startPanel;
    [SerializeField] private GameObject m_completePanel;
    [Tooltip("UI element that shows the active skill invoked by K key.")]
    [SerializeField] private GameObject m_activeK;

    [Space(10)]
    [Header("Player and Character")]
    [SerializeField] private GameObject m_player;

    [Space(10)]
    [Header("Gameplay and level")]
    [SerializeField] private float m_maxTime = 45.0f;
    [Tooltip("Start level, it can be any number other than 1 if configured.")]
    [Range(1, 4)]
    [SerializeField] private int m_levelNum = 1;
    private float m_timer = 0.0f;
    private MapManager m_mapManager;

    //delegate for dynamic button action assignment
    delegate void ButtonAction();
    ButtonAction m_actionItem;
    ButtonAction m_actionAttribute;

    public GameObject GetCharacterObject()
    {
        return m_player.GetComponent<Player>().GetCharacter();
    }

    public Character GetCharacter()
    {
        return GetCharacterObject() ? GetCharacterObject().GetComponent<Character>() : null;
    }

    struct Item
    {
        public string m_name;
        public int m_index;
        public bool m_active;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_levelText.text = "Level " + m_levelNum.ToString();
        m_mapManager = gameObject.GetComponent<MapManager>();
        LoadLevel(m_levelNum);
        m_mapManager.LoadLevel(m_levelNum);
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        float timeLeft = m_maxTime - m_timer;
        m_timerText.text = Mathf.Round(timeLeft).ToString() + "s";
        if (GetCharacter() && GetCharacter().GetComponent<Character>().GetHealth() <= 0.0f)
        {
            ResetGame(1);
            return;
        }
        if (timeLeft <= 0)
        {
            Time.timeScale = 0.0f;
            m_timer = 0.0f;

            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Disposable"))
            {
                Destroy(o);
            }

            Shop();
        }

    }

    void Shop()
    {
        m_player.transform.position = Vector3.zero;
        m_gameplayPanel.SetActive(false);
        m_shopPanel.SetActive(true);
        GameObject attributeButton = m_shopPanel.transform.GetChild(0).gameObject;
        GameObject itemButton = m_shopPanel.transform.GetChild(1).gameObject;
        Character c = GetCharacter();

        //show hp recovery regardless of the level
        int hpRecovery = Random.Range(15, 36);
        string description = "Recover " + hpRecovery.ToString() + " HP";
        attributeButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = description;
        m_actionAttribute = delegate ()
        {
            if (c)
            {
                c.SetHealth(c.GetHealth() + (float)hpRecovery);
            }
        };

        //show items based on the level
        switch (m_levelNum)
        {
            case 1:
                {
                    Item item = new Item();
                    item.m_name = "Trainbound";
                    item.m_index = 2;
                    item.m_active = true;

                    itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.m_name;
                    m_actionItem = delegate ()
                    {
                        if (c)
                        {
                            c.GiveActiveItem<ActiveItem_2>(KeyCode.K);
                            m_activeK.SetActive(true);
                        }
                    };
                    break;
                }

            case 2:
                gameObject.GetComponent<EnemySpawn>().enabled = true;
                CloseShop();
                break;
            case 3:
                {
                    Item item = new Item();
                    item.m_name = "Toxic Footprint";
                    item.m_index = 0;
                    item.m_active = false;

                    itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.m_name;
                    m_actionItem = delegate ()
                    {
                        if (c)
                        {
                            c.GivePassiveItem<PassiveItem_0>();
                        }
                    };
                    break;
                }

            case 4:
                GetComponent<SendToGoogle>().Send(m_levelNum, Random.Range(0, 5), Random.Range(0, 10));
                m_shopPanel.SetActive(false);
                m_completePanel.SetActive(true);
                break;
        }
    }

    void CloseShop()
    {
        m_gameplayPanel.SetActive(true);
        m_shopPanel.SetActive(false);
        m_levelNum++;
        ResetGame();
    }


    public void ResetGame()
    {
        m_levelText.text = "Level " + m_levelNum.ToString();
        m_mapManager.LoadLevel(m_levelNum);

        Time.timeScale = 1.0f;
    }

    public void ResetGame(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                {

                    GetComponent<SendToGoogle>().Send(m_levelNum, Random.Range(0, 5), Random.Range(0, 10));
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                    break;
                }

            default:
                m_levelNum = levelNum;
                LoadLevel(m_levelNum);
                ResetGame();
                break;
        }
    }
    
    public void TrackDamage(string element, float damageAmount)
    {
        GetComponent<SendToGoogle>().SendDamageData(element, damageAmount);
        
    }

    public int GetLevelNum()
    {
        return m_levelNum;
    }

    public void ItemButtonOnClick()
    {

        if (m_actionItem != null)
        {
            m_actionItem();
        }
        CloseShop();
    }

    public void AttributeButtonOnClick()
    {
        if(m_actionAttribute != null)
        {
            m_actionAttribute();
        }
        CloseShop();
    }

    public void StartButtonOnClick()
    {
        m_startPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    
    public void TrackDamage(string element, float damageAmount)
    {
        GetComponent<SendToGoogle>().SendDamageData(element, damageAmount);

        
    }

    void LoadLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                break;
            case 2:
                if (GetCharacter())
                {
                    GetCharacter().GiveActiveItem<ActiveItem_2>(KeyCode.K);
                    m_activeK.SetActive(true);
                }
                break;
            case 3:
                LoadLevel(2);
                gameObject.GetComponent<EnemySpawn>().enabled = true;
                break;
            case 4:
                LoadLevel(3);
                if (GetCharacter())
                {
                    GetCharacter().GivePassiveItem<PassiveItem_0>();
                }
                break;
        }
    }
}
