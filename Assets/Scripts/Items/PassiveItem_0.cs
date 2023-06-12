using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_0 : PassiveItem
{
    private static string prefabPath = "Prefabs/Footprint";
    private Vector3 prevPos;
    private float stepsize;
    private float minStepsize = 0.5f;
    private float maxStepsize = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ((transform.position - prevPos).magnitude >= stepsize)
        {
            GameObject footprint = Instantiate(Resources.Load<GameObject>(prefabPath), transform.position + Vector3.forward * GameObject.Find("GameplayManager").transform.position.z / 2, transform.rotation);
            DestroyOutOfTime timer = footprint.AddComponent<DestroyOutOfTime>();
            timer.SetTimer(5.0f);
            timer.Activate();
            prevPos = transform.position;
            stepsize = Random.Range(minStepsize, maxStepsize);
        }
    }
}
