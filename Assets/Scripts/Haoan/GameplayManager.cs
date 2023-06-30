using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    [SerializeField] private GameObject m_player;
    private Character m_character;
    [SerializeField] private GameObject m_gameplayPanel;
    [SerializeField] private GameObject m_shopPanel;
    [SerializeField] private GameObject m_startPanel;
    [SerializeField] private GameObject m_completePanel;
    [SerializeField] private GameObject m_activeK;
    [SerializeField] private float m_maxTime = 45.0f;
    [SerializeField] private int m_levelNum = 1;


    private float m_timer = 0.0f;
    private MapManager m_mapManager;
    private GameObject m_jack;
    private GameObject m_king;
    private GameObject m_lord;

    private List<Item> m_item;

    delegate void PositionAction();
    PositionAction m_actionJack;
    PositionAction m_actionKing;
    PositionAction m_actionLord;

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

        levelText.text = "Level " + m_levelNum.ToString();
        m_mapManager = gameObject.GetComponent<MapManager>();
        Player player = m_player.GetComponent<Player>();
        m_king = player.GetClosestCharacter(0);
        m_character = m_king.GetComponent<Character>();
        m_mapManager.LoadLevel(m_levelNum);
        LoadLevel(m_levelNum);
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        float timeLeft = m_maxTime - m_timer;
        timerText.text = Mathf.Round(timeLeft).ToString() + "s";
        if (timeLeft <= 0)
        {
            Time.timeScale = 0.0f;
            m_timer = 0.0f;
            Shop();
        }
    }

    void Shop()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Disposable"))
        {
            Destroy(o);
        }
        m_player.transform.position = Vector3.zero;

        m_gameplayPanel.SetActive(false);
        m_shopPanel.SetActive(true);
        GameObject itemButton = m_shopPanel.transform.GetChild(2).gameObject;

        if (m_levelNum == 1)
        {
            //select random item
            Item item = new Item();
            item.m_name = "Dash";
            item.m_index = 2;
            item.m_active = true;

            itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.m_name;
            m_actionItem = delegate ()
            {
                if (m_character)
                {
                    m_character.GiveItem(typeof(ActiveItem_2));
                    m_activeK.SetActive(true);
                }
            };
        }
        else if (m_levelNum == 2)
        {
            gameObject.GetComponent<SpawnManager>().enabled = true;
            CloseShop();
        }
        else if (m_levelNum == 3)
        {
            //select random item
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
        levelText.text = "Level " + m_levelNum.ToString();
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
            ResetGame();
        }
    }

    public int GetLevelNum()
    {
        return m_levelNum;
    }

    //public void AttributeButtonOnClick()
    //{
    //    CloseShop();
    //}

    //public void CharacterButtonOnClick()
    //{
    //    if(m_levelNum == 1)
    //    {
    //        m_characters[1].SetActive(true);
    //        Player player = m_player.GetComponent<Player>();
    //        m_jack = player.GetClosestCharacter(1);

    //    }
    //    else if(m_levelNum == 2)
    //    {
    //        m_characters[2].SetActive(true);
    //        Player player = m_player.GetComponent<Player>();
    //        m_lord = player.GetClosestCharacter(2);
    //    }
    //    CloseShop();
    //}

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
        if(levelNum == 1)
        {
            //do nothing
        }
        else if(levelNum == 2)
        {
            if (m_character)
            {
                m_character.GiveItem(typeof(ActiveItem_2));
                m_activeK.SetActive(true);
            }
        }
        else if(levelNum == 3)
        {
            LoadLevel(2);
            gameObject.GetComponent<SpawnManager>().enabled = true;
        }
        else if(levelNum == 4)
        {
            LoadLevel(3);
            if (m_character)
            {
                m_character.GiveItem(typeof(PassiveItem_0));
            }
        }
    }
}
