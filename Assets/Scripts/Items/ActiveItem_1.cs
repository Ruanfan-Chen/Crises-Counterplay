using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_1 : MonoBehaviour, IItem
{
    private float angularVelocity = 180.0f;
    private float angularDisplacement = 120.0f;
    public void Activate()
    {
        StartCoroutine(RotatePlayer());
    }

    IEnumerator RotatePlayer()
    {
        float cumulativeAD = 0.0f;
        while (cumulativeAD + angularVelocity * Time.deltaTime <= angularDisplacement)
        {
            cumulativeAD += angularVelocity * Time.deltaTime;
            GetComponentInParent<Player>().Rotate(Quaternion.Euler(0, 0, angularVelocity * Time.deltaTime));
            yield return null;
        }
        GetComponentInParent<Player>().Rotate(Quaternion.Euler(0, 0, angularDisplacement - cumulativeAD));
    }
}
