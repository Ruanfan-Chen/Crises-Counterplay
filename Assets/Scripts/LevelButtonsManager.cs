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

    // public static Dictionary<int, int> nextLevels = new Dictionary<int, int>()
    // // Need to update when more complex / higher levels have been desiged
    // {
    //     {0,1},
    //     {1,2},
    //     {2,3},
    //     {5, 8},
    //     {6, 9},
    //     {7, 10},
    //     {8, 11},
    //     {9, 11},
    //     {10, 11}
    //     // {8, 11},
    //     // {9, 11},
    //     // {10, 13},
    //     // {11, new List<int> {8, 9} },
    //     // {12, new List<int> {8, 10} },
    //     // {13, new List<int> {9, 10} },
    //     // {14, new List<int> {11, 12, 13} }
    // };

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
                buttons[levelNum].onClick.AddListener(() => { LevelManager.SetLevelNum(levelNum); SceneManager.LoadScene(sceneName); LevelManager.SetLevelNum(levelNum); Debug.Log("Clicked button i = " + levelNum + ". Now level number = " + LevelManager.GetLevelNum()); currLevel = levelNum; });
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
                    GameObject.FindGameObjectWithTag("Button" + i).GetComponent<Button>().onClick.AddListener(() => { LevelManager.SetLevelNum(levelNum); SceneManager.LoadScene(sceneName); LevelManager.SetLevelNum(levelNum); Debug.Log("Clicked button i = " + levelNum + ". Now level number = " + LevelManager.GetLevelNum()); currLevel = levelNum; });
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
