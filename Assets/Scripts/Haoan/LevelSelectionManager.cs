using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public Button[] levelButtons; // length = 13
    public static Dictionary<int, List<int>> levelsPrereqs = new Dictionary<int, List<int>>()
    {
        {2, new List<int> {} },
        {3, new List<int> {} },
        {4, new List<int> {} },
        {5, new List<int> {2} },
        {6, new List<int> {3} },
        {7, new List<int> {4} },
        {8, new List<int> {5} },
        {9, new List<int> {6} },
        {10, new List<int> {7} },
        {11, new List<int> {8, 9} },
        {12, new List<int> {8, 10} },
        {13, new List<int> {9, 10} },
        {14, new List<int> {11, 12, 13} }
    };





    // Start is called before the first frame update
    void Start()
    {

        UpdateButtons();
        string[] levelNames = { "Train1", "Thunder1", "Tsunami1", "Train2", "Thunder2", "Tsunami2", "Train3", "Thunder3", "Tsunami3", "TrainThunder", "TrainTsunami", "ThunderTsunami", "FullLevel" };
        print("levelNames[0] = " + levelNames[0]);
        // for (int i = 0; i < levelButtons.Length; i++)
        // {
        //     Debug.Log("levelNames[i] = " + levelNames[i]);
        //     Debug.Log("GameObject.FindGameObjectWithTag = " + GameObject.FindGameObjectWithTag("Button" + (i + 2)));
        //     GameObject.FindGameObjectWithTag("Button" + (i + 2)).GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(levelNames[i]));

        //     Debug.Log("set onclick for button i = " + (i + 2));
        // }

        // TODO: Ugly codes, need to resolve OutOfBound error occurs above
        GameObject.FindGameObjectWithTag("Button2").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(2); });
        GameObject.FindGameObjectWithTag("Button3").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(3); });
        GameObject.FindGameObjectWithTag("Button4").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(4); });
        GameObject.FindGameObjectWithTag("Button5").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(5); });
        GameObject.FindGameObjectWithTag("Button6").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(6); });
        GameObject.FindGameObjectWithTag("Button7").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(7); });
        GameObject.FindGameObjectWithTag("Button8").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(8); });
        GameObject.FindGameObjectWithTag("Button9").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(9); });
        GameObject.FindGameObjectWithTag("Button10").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(10); });
        GameObject.FindGameObjectWithTag("Button11").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(11); });
        GameObject.FindGameObjectWithTag("Button12").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(12); });
        GameObject.FindGameObjectWithTag("Button13").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(13); });
        GameObject.FindGameObjectWithTag("Button14").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); NextLevelManager.SetCurrLevel(14); });




    }

    void Update()
    {
        UpdateButtons();
    }



    private void UpdateButtons()
    // TODO: make sure levelNum is within the range [2, 14]. Might need alignments in MapManager.cs
    {
        Debug.Log("in UpdateButtons(), levelButtons.Length = " + levelButtons.Length);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelsPrereqs[i + 2].AsQueryable().Any(prereq => !NextLevelManager.completed.Contains(prereq)))
            {
                Debug.Log("button set to false : " + i);
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }

        foreach (int levelNum in NextLevelManager.completed)
        {
            GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().GetComponent<Image>().color = Color.green;
            Debug.Log("Button set to green: " + levelNum);
            GameObject.FindGameObjectWithTag("Button" + levelNum).GetComponent<Button>().interactable = false;
        }

    }

}
