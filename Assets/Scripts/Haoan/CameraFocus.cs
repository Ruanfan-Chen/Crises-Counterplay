using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    private GameObject focus;
    private Vector3 offset = new Vector3(0.0f, 0.0f, -50.0f);

    public GameObject GetFocus()
    {
        return focus;
    }

    public void SetFocus(GameObject value)
    {
        focus = value;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = focus.transform.position + offset;
    }
}
