using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_1 : MonoBehaviour, IItem
{
    private float angularVelocity = 180.0f;
    public void Activate()
    {
        StartCoroutine(RotatePlayer());
    }

    IEnumerator RotatePlayer()
    {
        float angularDisplacement = 0.0f;
        while (angularDisplacement + angularVelocity * Time.deltaTime <= 120.0f)
        {
            angularDisplacement += angularVelocity * Time.deltaTime;
            GetComponentInParent<Player>().Rotate(Quaternion.Euler(0, 0, angularVelocity * Time.deltaTime));
            yield return null;
        }
        GetComponentInParent<Player>().Rotate(Quaternion.Euler(0, 0, 120.0f - angularDisplacement));
    }
}
