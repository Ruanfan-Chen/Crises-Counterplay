using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] private GameObject m_gameplayPanel;
    [SerializeField] private GameObject m_shopPanel;
    [SerializeField] private GameObject m_startPanel;
    [SerializeField] private GameObject m_completePanel;
    [Tooltip("UI element that shows the active skill invoked by pressing the keys.")]
    [SerializeField] private GameObject m_activeJ;
    [SerializeField] private GameObject m_activeK;
    [SerializeField] private GameObject m_activeL;
    [SerializeField] private GameObject m_manaBar;

    [Space(10)]
    [Header("Player and Character")]
    [SerializeField] private GameObject m_player;

    [Space(10)]
    [Header("Gameplay and level")]
    [SerializeField] private float m_maxTime = 45.0f;
    [Tooltip("Start level, it can be any number other than 1 if configured.")]
    [SerializeField] private int m_levelNum = 1;
    private float m_timer = 0.0f;
    private MapManager m_mapManager;
    private int m_mapToLoad;
    private bool m_isPaused = false;
    private GameObject m_goal = null;

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

    // Start is called before the first frame update
    void Start()
    {
        m_levelText.text = "Level " + m_levelNum.ToString();
        m_mapManager = gameObject.GetComponent<MapManager>();

        m_mapManager.LoadLevel(m_levelNum);
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_goal == null)
        {
            m_timer += Time.deltaTime;
        }
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
        //int hpRecovery = Random.Range(15, 36);
        //string description = "+" + hpRecovery.ToString() + " HP";
        //attributeButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = description;
        //m_actionAttribute = delegate ()
        //{
        //    if (c)
        //    {
        //        c.SetHeah(c.GetHealth() + (float)hpRecovery);
        //    }
        //};

        //show ims based on the level
        switch (m_levelNum)
        {
            case 1:
                {
                    ActiveItem candidate = null;
                    int randint = Random.Range(0, 2);
                    switch (randint)
                    {
                        case 0:
                            candidate = new ActiveItem_2();
                            itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ActiveItem_2.GetName();
                            itemButton.GetComponent<Image>().sprite = ActiveItem_2.GetLogo();
                            break;
                        case 1:
                            candidate = new ActiveItem_2_0();
                            itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ActiveItem_2_0.GetName();
                            itemButton.GetComponent<Image>().sprite = ActiveItem_2_0.GetLogo();
                            break;
                    }

                    attributeButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ActiveItem_2.GetName();
                    attributeButton.GetComponent<Image>().sprite = ActiveItem_2.GetLogo();
                    m_actionAttribute = delegate ()
                    {
                        GiveTrainActive(0);
                        m_mapToLoad = 110;
                        Vector3 enemyPos = new Vector3(-15.0f, 0.0f, 0.0f);
                        GameObject enemy = Enemy.Instantiate(enemyPos, Quaternion.identity);
                        enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                        m_goal = enemy;
                        PassiveItem_Weapon_0 gun = GetCharacterObject().GetComponent<PassiveItem_Weapon_0>();
                        Destroy(gun);
                    };

                    itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ActiveItem_2_0.GetName();
                    itemButton.GetComponent<Image>().sprite = ActiveItem_2_0.GetLogo();
                    m_actionItem = delegate ()
                    {
                        GiveTrainActive(1);
                        m_mapToLoad = 111;
                        Vector3 enemyPos = new Vector3(15.0f, 0.0f, 0.0f);
                        GameObject enemy = Enemy.Instantiate(enemyPos, Quaternion.identity);
                        enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                        m_goal = enemy;
                        PassiveItem_Weapon_0 gun = GetCharacterObject().GetComponent<PassiveItem_Weapon_0>();
                        Destroy(gun);
                    };

                    //itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ActiveItem_0.GetName();
                    //itemButton.GetComponent<Image>().sprite = ActiveItem_0.GetLogo();
                    //m_actionItem = delegate ()
                    //{
                    //    GiveElectricActive();
                    //    m_mapToLoad = 22;
                    //};
                    break;
                }

            case 2:
                gameObject.GetComponent<EnemySpawn>().enabled = true;
                CloseShop();
                break;
            case 3:
                {
                    itemButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Toxic Footprint";
                    itemButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Skills/Toxic Footprint");
                    m_actionItem = delegate ()
                    {
                        GiveToxicFootprint();
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
        m_mapManager.LoadLevel(m_mapToLoad);

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
        if (m_actionAttribute != null)
        {
            m_actionAttribute();
        }
        CloseShop();
    }

    public void StartButtonOnClick()
    {
        m_startPanel.SetActive(false);
        LoadLevel(m_levelNum);
        Time.timeScale = 1.0f;
    }

    void LoadLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                break;
            case 2:
                GiveTrainActive(1);
                break;
            case 3:
                LoadLevel(2);
                gameObject.GetComponent<EnemySpawn>().enabled = true;
                break;
            case 4:
                LoadLevel(3);
                GiveToxicFootprint();
                break;
            case 111:
                //Chistrike level
                GiveTrainActive(1);
                m_mapManager.LoadLevel(21);
                CrisisManager crisisManager = gameObject.GetComponent<CrisisManager>();
                crisisManager.SetTrainWeight(1.0f);
                crisisManager.SetThunderWeight(0.0f);
                crisisManager.SetWaveWeight(0.0f);
                break;
        }
    }

    void GiveTrainActive(int seed)
    {
        Character c = GetCharacter();
        if (c)
        {
            int randint;
            if (seed == -1)
            {
                randint = Random.Range(0, 2);
            }
            else
            {
                randint = seed;
            }
            switch (randint)
            {
                case 0:
                    {
                        ActiveItem_2 activeItem = GetCharacter().GiveItem<ActiveItem_2>(KeyCode.K);
                        m_activeK.SetActive(true);
                        m_activeK.GetComponent<SpellCooldown>().SetActiveItem(activeItem, ActiveItem_2.GetLogo());
                        m_activeK.GetComponent<SpellCooldown>().SetCooldownTime(3.0f);
                        m_manaBar.SetActive(true);
                        m_manaBar.transform.GetChild(0).gameObject.GetComponent<Bar>().SetTarget(m_player);
                        break;
                    }

                case 1:
                    {
                        ActiveItem_2_0 activeItem = GetCharacter().GiveItem<ActiveItem_2_0>(KeyCode.K);
                        m_activeK.SetActive(true);
                        m_activeK.GetComponent<SpellCooldown>().SetActiveItem(activeItem, ActiveItem_2_0.GetLogo());
                        m_activeK.GetComponent<SpellCooldown>().SetCooldownTime(5.0f);
                        m_manaBar.SetActive(true);
                        m_manaBar.transform.GetChild(0).gameObject.GetComponent<Bar>().SetTarget(m_player);
                        break;
                    }
            }

        }
    }

    void GiveElectricActive()
    {
        ActiveItem_0 activeItem = GetCharacter().GiveItem<ActiveItem_0>(KeyCode.J);
        m_activeJ.SetActive(true);
        m_activeJ.GetComponent<Supercharge>().SetActiveItem(activeItem, ActiveItem_0.GetLogo());
    }

    void GiveToxicFootprint()
    {
        if (GetCharacter())
        {
            GetCharacter().GiveItem<PassiveItem_0>();
        }
    }
}
