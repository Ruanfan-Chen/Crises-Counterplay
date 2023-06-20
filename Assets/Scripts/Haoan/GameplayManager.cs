using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject[] m_characters;
    [SerializeField] private GameObject m_gameplayPanel;
    [SerializeField] private GameObject m_shopPanel;
    [SerializeField] private GameObject m_positionPanel;
    [SerializeField] private float m_maxTime = 45.0f;
    [SerializeField] private int m_levelNum = 1;
    private float m_timer = 0.0f;
    private MapManager m_mapManager;
    private GameObject m_jack;
    private GameObject m_king;
    private GameObject m_lord;

    private Item m_item;

    delegate void PositionAction();
    PositionAction m_actionJack;
    PositionAction m_actionKing;
    PositionAction m_actionLord;

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
        m_mapManager.LoadLevel(m_levelNum);
        Player player = m_player.GetComponent<Player>();
        m_king = player.GetClosestCharacter(0);
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        float timeLeft = m_maxTime - m_timer;
        timerText.text = Mathf.Round(timeLeft).ToString()+"s";
        if (timeLeft <= 0)
        {
            Time.timeScale = 0.0f;
            Shop();
        }
    }

    void Shop()
    {
        m_gameplayPanel.SetActive(false);
        m_shopPanel.SetActive(true);
        GameObject itemButton = m_shopPanel.transform.GetChild(2).gameObject;

        Item item = new Item();
        item.m_name = "Passive Item 1";
        item.m_index = 1;
        item.m_active = false;
        m_item = item;

        itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = item.m_name;
    }

    void CloseShop()
    {
        m_gameplayPanel.SetActive(true);
        m_shopPanel.SetActive(false);
        m_positionPanel.SetActive(false);
        m_levelNum++;
        levelText.text = "Level " + m_levelNum.ToString();
        ResetGame();
    }

    void PositionPage()
    {
        m_shopPanel.SetActive(false);
        m_positionPanel.SetActive(true);
    }


    public void ResetGame()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Disposable"))
        {
            Destroy(o);
        }

        m_player.transform.position = Vector3.zero;
        m_mapManager.LoadLevel(m_levelNum);
        
        if (m_jack)
        {
            Character character = m_jack.GetComponent<Character>();
            if (character)
            {
                character.SetHealth(character.GetMaxHealth());
            }
        }
        if (m_king)
        {
            Character character = m_king.GetComponent<Character>();
            if (character)
            {
                character.SetHealth(character.GetMaxHealth());
            }
        }
        if (m_lord)
        {
            Character character = m_lord.GetComponent<Character>();
            if (character)
            {
                character.SetHealth(character.GetMaxHealth());
            }
        }

        Time.timeScale = 1.0f;
        m_timer = 0.0f;
    }

    public void ResetGame(int levelNum)
    {
        m_levelNum = levelNum;
        ResetGame();
    }

    public void AttributeButtonOnClick()
    {
        CloseShop();
    }

    public void CharacterButtonOnClick()
    {
        if(m_levelNum == 1)
        {
            m_characters[1].SetActive(true);
            Player player = m_player.GetComponent<Player>();
            m_jack = player.GetClosestCharacter(1);
            
        }
        else if(m_levelNum == 2)
        {
            m_characters[2].SetActive(true);
            Player player = m_player.GetComponent<Player>();
            m_lord = player.GetClosestCharacter(2);
        }
        CloseShop();
    }

    public void ItemButtonOnClick()
    {
        m_actionJack = delegate () {
            if (m_jack)
            {
                Character character = m_jack.GetComponent<Character>();
                if (character)
                {
                    character.GiveItem(typeof(PassiveItem_0));
                }
            }
        };
        m_actionKing = delegate ()
        {
            if (m_king)
            {
                Character character = m_king.GetComponent<Character>();
                if (character)
                {
                    character.GiveItem(typeof(PassiveItem_0));
                }
            }
        };
        m_actionLord = delegate ()
        {
            if (m_lord)
            {
                Character character = m_lord.GetComponent<Character>();
                if (character)
                {
                    character.GiveItem(typeof(PassiveItem_0));
                }
            }
        };
        PositionPage();
    }

    public void JackOnClick()
    {
        if (m_actionJack != null)
        {
            m_actionJack();
        }
        CloseShop();
    }

    public void KingOnClick()
    {
        if(m_actionLord != null)
        {
            m_actionKing();
        }
        CloseShop();
    }

    public void LordOnClick()
    {
        if(m_actionLord != null)
        {
            m_actionLord();
        }
        CloseShop();
    }
}
