using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonsManager : MonoBehaviour
{
    public static int numOfLevels = 13;

    public static Dictionary<int, List<int>> levelsPrereqs = new Dictionary<int, List<int>>()
    {   {0, new List<int> {} },
        {1, new List<int> {0} },
        {2, new List<int> {1} },
        {3, new List<int> {} },
        {4, new List<int> {3} },
        {5, new List<int> {4} },
        {6, new List<int> {} },
        {7, new List<int> {6} },
        {8, new List<int> {7} },
        {9, new List<int> {2, 5} },
        {10, new List<int> {2, 8} },
        {11, new List<int> {5, 8} },
        {12, new List<int> {9, 10, 11} }
    };

    public static HashSet<int> completed = new HashSet<int>() { };

    public static bool updated;
    private static bool shouldReset;
    private static bool infiniteLevelAttempted;
    private List<GameObject> infLevelDependencies;
    public bool inspectorMode;



    void Start()
    {
        if (inspectorMode)
        {
            for (int levelNum = 0; levelNum < numOfLevels; levelNum++)
                ActivateButton(levelNum, Color.white);
        }
        else
        {
            ActivateButton(0, Color.white);
            ActivateButton(3, Color.white);
            ActivateButton(6, Color.white);
            // updated = false;
            infLevelDependencies = new List<GameObject> { };
            foreach (int levelNum in levelsPrereqs[numOfLevels - 1])
                infLevelDependencies.Add(GameObject.FindWithTag("D" + levelNum));
            infiniteLevelAttempted = false;
        }

    }

    void Update()
    {
        UpdateButtons();
    }


    // Update is called once per frame
    public void UpdateButtons()

    {
        if (!inspectorMode)
        {
            if (shouldReset)
            {
                for (int levelNum = 0; levelNum < numOfLevels; levelNum++)
                {
                    // Debug.Log("levelNum = " + levelNum + ", number of resets = " + GameObject.FindGameObjectsWithTag("D" + levelNum).Length);
                    GameObject[] objs = GameObject.FindGameObjectsWithTag("D" + levelNum);
                    foreach (GameObject obj in objs)
                    {
                        obj.GetComponent<Image>().GetComponent<Image>().color = Color.white;
                    }
                }
                // Debug.Log("reset 0 color = " + GameObject.FindGameObjectWithTag("D0").GetComponent<Image>().GetComponent<Image>().color);
                shouldReset = false;
                updated = false;

            }

            if (!updated)
            {
                UIManager.m_completePanel.SetActive(false);
                UIManager.m_losePanel.SetActive(false);
                for (int levelNum = 0; levelNum < numOfLevels; levelNum++)
                {
                    bool advancable = true;
                    foreach (int prereq in levelsPrereqs[levelNum])
                    {
                        if (!completed.Contains(prereq))
                            advancable = false;
                    }
                    if (!advancable)
                    {
                        //Debug.Log("level " + levelNum + " cannot be played, deactivated");
                        DeactivateButton(levelNum, Color.white);
                    }
                    else
                    {
                        //Debug.Log("level " + levelNum + " can be played, activated");
                        ActivateButton(levelNum, Color.white);
                    }

                }

                for (int levelNum = 0; levelNum < numOfLevels; levelNum++)
                {
                    if (completed.Contains(levelNum))
                    {
                        // Debug.Log("level " + levelNum + " is in completed, deactivated to green");
                        DeactivateButton(levelNum, Color.green);
                    }

                }
                ShowDependencies(infLevelDependencies);
                updated = true;
                //Debug.Log("completed length = " + completed.Count);

            }

            if (!infiniteLevelAttempted)
            {
                ActivateButton(12, Color.white);
                infiniteLevelAttempted = true;
                HideDependencies(infLevelDependencies);
                updated = true;
            }
        }

    }

    private void ActivateButton(int levelNum, Color color)
    {
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().interactable = true;
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().onClick.AddListener(() =>
        {
            LevelManager.SetLevelNum(levelNum);
            //Debug.Log("cLicked button " + levelNum);
            UIManager.m_levelSelectionPanel.SetActive(false);
            UIManager.m_activeSkillPanel.SetActive(true);
            CrisisManager.Activate();
            GameplayManager.LoadLevel();
            GameplayManager.Continue();
        });
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().GetComponent<Image>().color = color;
        if (levelNum < numOfLevels - 1)
            GameObject.FindGameObjectsWithTag("D" + levelNum).Select(obj => obj.GetComponent<Image>().GetComponent<Image>().color = color);

        // GameObject.FindGameObjectsWithTag("D" + levelNum).Select(obj => obj.GetComponent<Image>().GetComponent<Image>().color = color);
        // GameObject.FindGameObjectWithTag("D" + levelNum).GetComponent<Image>().GetComponent<Image>().color = color;

    }

    private void DeactivateButton(int levelNum, Color color)
    {
        //Debug.Log("button set to false : " + levelNum);
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().GetComponent<Image>().color = color;
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().interactable = false;
        GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().onClick.RemoveAllListeners();
        if (levelNum < numOfLevels - 1)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("D" + levelNum))
            {
                obj.GetComponent<Image>().GetComponent<Image>().color = color;
            }
        }

    }


    public static void AddCompletedLevel()
    {
        completed.Add(LevelManager.GetLevelNum());

    }

    public static void ResetCompletedLevels()
    {
        completed = new HashSet<int>() { };
        shouldReset = true;
        updated = false;


    }

    private void ShowDependencies(List<GameObject> dependencies)
    {
        foreach (GameObject obj in dependencies)
            obj.SetActive(true);
    }

    private void HideDependencies(List<GameObject> dependencies)
    {
        foreach (GameObject obj in dependencies)
            obj.SetActive(false);
    }

}
