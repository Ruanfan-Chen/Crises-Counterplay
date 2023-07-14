using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public static Button[] levelButtons; // length = 13
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

    public static HashSet<int> completed = new HashSet<int>() { };

    // Start is called before the first frame update
    void Start()
    {
        // int currLevel = PlayerPrefs.GetInt("levelAt", 2);
        updateButtons();
        SendToGoogle.Initialize();;
    }

    public static void addCompletedLevel(int levelNum)
    {
        completed.Add(levelNum);
        levelButtons[levelNum - 2].GetComponent<Image>().color = Color.green;
        updateButtons();
        Character.damageCount = 0;
    }

    private static void updateButtons()
    // TODO: make sure levelNum is within the range [2, 14]. Might need alignments in MapManager.cs
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelsPrereqs[i + 2].AsQueryable().Any(prereq => !completed.Contains(prereq)))
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }





    }
}
