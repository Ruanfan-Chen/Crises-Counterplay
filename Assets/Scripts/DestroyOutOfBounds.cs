using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    internal GameObject gameplayManager;
    private float offset = -15.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameplayManager.GetComponent<MapManager>().IsInMap(transform.position, offset))
            Destroy(gameObject);
    }
}
