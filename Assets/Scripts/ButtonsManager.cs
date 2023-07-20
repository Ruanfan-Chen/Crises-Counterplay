using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.m_completePanel = GameObject.FindWithTag("CompletePanel");
        UIManager.m_levelSelectionPanel = GameObject.FindWithTag("LevelSelectionPanel");
        UIManager.m_losePanel = GameObject.FindWithTag("LosePanel");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackToMain()
    {
        GameplayManager.Pause();
        CrisisManager.Deactivate();
        UIManager.m_completePanel.SetActive(false);
        UIManager.m_losePanel.SetActive(false);
        UIManager.m_levelSelectionPanel.SetActive(true);
    }

}