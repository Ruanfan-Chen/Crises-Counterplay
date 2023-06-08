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
    private float m_timer = 0.0f;
    private int m_levelNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        levelText.text = "Level " + m_levelNum.ToString();
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

    void ResetGame()
    {
        m_player.transform.position = Vector3.zero;
        MapManager mm = gameObject.GetComponent<MapManager>();
        mm.CreateMap(MapManager.Shape.Circle);
        Time.timeScale = 1.0f;
        m_timer = 0.0f;
        GameObject disposable = GameObject.Find("disposable");
        Debug.Log(disposable == null);
        if(disposable)
        {
            Debug.Log("destroy");
            Destroy(disposable);
        }
    }

    public void CharacterButtonOnClick()
    {
        m_characters[1].SetActive(true);
        CloseShop();
    }
}
