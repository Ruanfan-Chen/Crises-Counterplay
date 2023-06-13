using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfTime : MonoBehaviour
{
    private float timer = 0.0f;
    private bool active = false;

    public float GetTimer() { return timer; }

    public void SetTimer(float value) { timer = value; }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            SetTimer(GetTimer() - Time.deltaTime);
            if (GetTimer() < 0)
                Destroy(gameObject);
        }
    }

    public void Activate() { active = true; }

    public void Deactivate() { active = false; }
}
