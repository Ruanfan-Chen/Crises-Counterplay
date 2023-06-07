using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public HashSet<int> passiveItems = new HashSet<int>();
    private int activeItem = 0;

    public int GetActiveItem()
    {
        return activeItem;
    }

    public void SetActiveItem(int value)
    {
        activeItem = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
