using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public GameObject footprintPrefab;
    private HashSet<int> passiveItems = new HashSet<int>();
    private int activeItem = 0;
    private Vector3 lastFootprintPos = Vector3.zero;
    private float stepSize = 1.0f;

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
        //passiveItems.Add(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (passiveItems.Contains(1) && (transform.position - lastFootprintPos).magnitude > stepSize)
        {
            GameObject footprint = Instantiate(footprintPrefab, transform.position, transform.rotation);
            footprint.GetComponent<DestroyOutOfTime>().SetTimer(5.0f);
            footprint.GetComponent<DestroyOutOfTime>().Activate();
            footprint.GetComponent<Faction>().SetHostility(false);
            footprint.tag = "Disposable";
            lastFootprintPos = footprint.transform.position;
        }
    }

    public void ActivateItem()
    {
        switch (activeItem)
        {
            case 1:
                GetComponent<LaunchProjectile>().LaunchProjectileRing(GetComponent<CharacterLaunch>().projectilePrefab, 10);
                break;
            case 2:
                StartCoroutine(GetComponentInParent<PlayerCharacterPositioning>().Rotate(Quaternion.Euler(0, 0, 120)));
                break;
            case 3:
                StartCoroutine(GetComponentInParent<PlayerControl>().Dash());
                break;
        }
    }
}
