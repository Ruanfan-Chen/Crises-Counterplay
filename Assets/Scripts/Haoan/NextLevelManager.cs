using System.Collections;
using System.Collections.Generic;

public class NextLevelManager
{
    public static Dictionary<int, int> nextLevels = new Dictionary<int, int>()
    // Need to update when more complex / higher levels have been desiged
    {
        {2, 5 },
        {3, 6},
        {4, 7},
        {5, 8},
        {6, 9},
        {7, 10}
        // {8, 11},
        // {9, 11},
        // {10, 13},
        // {11, new List<int> {8, 9} },
        // {12, new List<int> {8, 10} },
        // {13, new List<int> {9, 10} },
        // {14, new List<int> {11, 12, 13} }
    };

    public static HashSet<int> completed = new HashSet<int>() { };

    public static void addCompletedLevel(int levelNum)
    {
        completed.Add(levelNum);
        // PrepareButtons();
        // Debug.Log("here we are adding levelNum =" + levelNum);
        // Debug.Log("levelButtons has length =" + levelButtons.Count);
        // Debug.Log("levelButtons[0] = " + levelButtons[0]);
        // levelButtons[levelNum - 2].GetComponent<Image>().color = Color.green;
        // UpdateButtons();

    }
}