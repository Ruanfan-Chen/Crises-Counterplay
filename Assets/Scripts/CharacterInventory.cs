using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    private HashSet<int> passiveItems = new HashSet<int>();
    private int activeItem = 0;

    public int GetActiveItem()
    {
        return activeItem;
    }

    public void SetActiveItem(int value)
    {
        activeItem = value;
    }

    public void AddPassiveItem(int i)
    {
        passiveItems.Add(i);
    }
    public HashSet<int> GetPassiveItems()
    {
        return passiveItems;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateItem() {
        Debug.Log("Item " + activeItem + " is activated.");
        switch (activeItem) {
            case 2:
                StartCoroutine(GetComponentInParent<PlayerCharacterPositioning>().Rotate(Quaternion.Euler(0, 0, 120)));
                break;
        }
    }
}
