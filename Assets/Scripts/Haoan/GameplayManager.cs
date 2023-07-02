using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private GameObject m_characterObject;
    private Character m_character;

    [Space(10)]
    [Header("Gameplay and level")]
    [SerializeField] private float m_maxTime = 45.0f;
    [Tooltip("Start level, it can be any number other than 1 if configured.")]
    [Range(1,4)]
    [SerializeField] private int m_levelNum = 1;
    private float m_timer = 0.0f;
    private MapManager m_mapManager;
    
    //delegate for dynamic button action assignment
    delegate void ItemAction();
    ItemAction m_actionItem;

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
        Player player = m_player.GetComponent<Player>();
        m_characterObject = player.GetClosestCharacter(0);
        m_character = m_characterObject.GetComponent<Character>();
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
        GameObject itemButton = m_shopPanel.transform.GetChild(1).gameObject;

        //show items based on the level
        if (m_levelNum == 1)
        {
            Item item = new Item();
            item.m_name = "Trainbound";
            item.m_index = 2;
            item.m_active = true;

            itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.m_name;
            m_actionItem = delegate ()
            {
                if (m_character)
                {
                    m_character.GiveItem(typeof(ActiveItem_3));
                    m_activeK.SetActive(true);
                }
            };
        }
        else if (m_levelNum == 2)
        {
            gameObject.GetComponent<EnemySpawn>().enabled = true;
            CloseShop();
        }
        else if (m_levelNum == 3)
        {
            Item item = new Item();
            item.m_name = "Toxic Footprint";
            item.m_index = 0;
            item.m_active = false;

            itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.m_name;
            m_actionItem = delegate ()
            {
                if (m_character)
                {
                    m_character.GiveItem(typeof(PassiveItem_0));
                }
            };

        }
        else if (m_levelNum == 4)
        {
            GetComponent<SendToGoogle>().Send(m_levelNum, Random.Range(0, 5), Random.Range(0, 10));
            m_shopPanel.SetActive(false);
            m_completePanel.SetActive(true);
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
        if (levelNum == 1)
        {

            GetComponent<SendToGoogle>().Send(m_levelNum, Random.Range(0, 5), Random.Range(0, 10));
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        else
        {
            m_levelNum = levelNum;
            LoadLevel(m_levelNum);
            ResetGame();
        }
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

    public void StartButtonOnClick()
    {
        m_startPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void LoadLevel(int levelNum)
    {
        if (levelNum == 1)
        {
            //do nothing
        }
        else if (levelNum == 2)
        {
            if (m_character)
            {
                m_character.GiveItem(typeof(ActiveItem_2));
                m_activeK.SetActive(true);
            }
        }
        else if (levelNum == 3)
        {
            LoadLevel(2);
            gameObject.GetComponent<EnemySpawn>().enabled = true;
        }
        else if (levelNum == 4)
        {
            LoadLevel(3);
            if (m_character)
            {
                m_character.GiveItem(typeof(PassiveItem_0));
            }
        }
    }
}
