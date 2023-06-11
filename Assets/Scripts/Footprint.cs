using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DestroyOutOfTime>().SetTimer(5.0f);
        GetComponent<DestroyOutOfTime>().Activate();
    }
}
