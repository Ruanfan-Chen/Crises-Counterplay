using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonsManager : MonoBehaviour
{
    public static int numOfLevels = 14;

    public Button[] buttons;

    public static Dictionary<int, List<int>> levelsPrereqs = new Dictionary<int, List<int>>()
    {   {0, new List<int> {} },
        {1, new List<int> {0} },
        {2, new List<int> {0} },
        {3, new List<int> {1,2} },
        {4, new List<int> {} },
        {5, new List<int> {4} },
        {6, new List<int> {5} },
        {7, new List<int> {} },
        {8, new List<int> {7} },
        {9, new List<int> {8} },
        {10, new List<int> {3, 6} },
        {11, new List<int> {3, 9} },
        {12, new List<int> {6, 9} },
        {13, new List<int> {10, 11, 12} }
    };

    public static HashSet<int> completed = new HashSet<int>() { };

    public static int currLevel;

    public static bool updated;



    void Start()
    {
        ActivateButton(0, Color.white);
        ActivateButton(4, Color.white);
        ActivateButton(7, Color.white);
        updated = false;
    }

    void Update()
    {
        UpdateButtons();
    }

    private void Initialize()
    {
        for (int i = 0; i < numOfLevels; i++)
        {
            int levelNum = i;
            Debug.Log("finding button " + i);
            if (levelNum == 0 || levelNum == 4 || levelNum == 7)
            {
                buttons[levelNum].onClick.AddListener(() =>
                {
                    Debug.Log("cLicked button " + i);
                    LevelManager.SetLevelNum(levelNum);
                    UIManager.m_levelSelectionPanel.SetActive(false);
                    GameplayManager.Continue();
                    GameplayManager.LoadLevel();
                    CrisisManager.Activate();
                    currLevel = levelNum;
                });
            }
            else
            {
                buttons[levelNum].onClick.AddListener(() => { });
            }

            Debug.Log("i = " + i + " and current level = " + LevelManager.GetLevelNum());
        }
        updated = false;
    }

    // Update is called once per frame
    public void UpdateButtons()

    {
        if (!updated)
        {
            for (int i = 0; i < numOfLevels; i++)
            {
                int levelNum = i;
                if (levelsPrereqs[i].AsQueryable().Any(prereq => !completed.Contains(prereq)))
                {
                    DeactivateButton(levelNum, Color.white);
                }
                else
                {
                    ActivateButton(levelNum, Color.white);
                }

            }

            for (int i = 0; i < numOfLevels; i++)
            {

                int levelNum = i;
                if (completed.Contains(i))
                {
                    DeactivateButton(levelNum, Color.green);
                }

            }
            updated = true;

        }
    }

    private void ActivateButton(int levelNum, Color color)
    {
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().interactable = true;
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().onClick.AddListener(() =>
        {
            LevelManager.SetLevelNum(levelNum);
            Debug.Log("cLicked button " + levelNum);
            currLevel = levelNum;
            UIManager.m_levelSelectionPanel.SetActive(false);
            CrisisManager.Activate();
            GameplayManager.LoadLevel();
            GameplayManager.Continue();
        });
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().GetComponent<Image>().color = color;
    }

    private void DeactivateButton(int levelNum, Color color)
    {
        Debug.Log("button set to false : " + levelNum);
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().GetComponent<Image>().color = color;
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().interactable = false;
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().onClick.RemoveAllListeners();
    }


    public static void AddCompletedLevel()
    {
        completed.Add(LevelManager.GetLevelNum());

    }

    public static void ResetCompletedLevels()
    {
        completed = new HashSet<int>() { };
        currLevel = 0;
        updated = false;

    }

}
