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
        for (int i = 0; i < numOfLevels; i++)
        {
            int levelNum = i;
            Debug.Log("finding button " + i);
            string sceneName = "Scene" + i;
            Debug.Log("sceneName = " + sceneName);
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

    void Update()
    {

        UpdateButtons();
    }

    // Update is called once per frame
    public void UpdateButtons()

    {
        if (!updated)
        {

            for (int i = 0; i < numOfLevels; i++)
            {
                string sceneName = "Scene" + i;
                int levelNum = i;
                if (GameObject.FindGameObjectWithTag("Button" + i) != null)
                {

                }
                if (levelsPrereqs[i].AsQueryable().Any(prereq => !completed.Contains(prereq)))
                {
                    Debug.Log("button set to false : " + i);
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().onClick.RemoveAllListeners();
                }
                else
                {
                    Debug.Log("button set to true : " + i);
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().interactable = true;
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        LevelManager.SetLevelNum(levelNum);
                        Debug.Log("cLicked button " + i);
                        currLevel = levelNum;
                        UIManager.m_levelSelectionPanel.SetActive(false);
                        CrisisManager.Activate();
                        GameplayManager.LoadLevel();
                        GameplayManager.Continue();
                    });
                }

            }

            for (int i = 0; i < numOfLevels; i++)
            {
                if (GameObject.FindGameObjectWithTag("Button" + i) != null)
                {

                }
                string sceneName = "Scene" + i;
                int levelNum = i;
                if (completed.Contains(i))
                {
                    Debug.Log("button set to false and green : " + i);
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().GetComponent<Image>().color = Color.green;
                    // Debug.Log("Button set to green: " + levelNum);
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().onClick.RemoveAllListeners();
                }

            }
            updated = true;

        }
        // 



    }


    public static void AddCompletedLevel()
    {
        completed.Add(LevelManager.GetLevelNum());

    }

    public static void ResetCompletedLevels()
    {
        completed = new HashSet<int>() { };
    }

}
