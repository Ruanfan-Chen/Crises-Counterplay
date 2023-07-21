using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaltTimer : MonoBehaviour
{
    private void OnEnable()
    {
        GameplayManager.AddHalt(this);
    }

    private void OnDisable()
    {
        GameplayManager.RemoveHalt(this);
    }
}
