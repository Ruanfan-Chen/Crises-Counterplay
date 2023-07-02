using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static T WeightedRandom<T>(Dictionary<T, float> weightDict)
    {
        float r = Random.Range(0.0f, weightDict.Sum(kvp => kvp.Value));
        foreach (KeyValuePair<T, float> kvp in weightDict)
        {
            if (r <= kvp.Value)
                return kvp.Key;
            else
                r -= kvp.Value;
        }
        return default;
    }

    public static IEnumerator ForcedMovement(Transform transform, Vector3 displacement, float initialSpeed, float duration) {
        float current = 0.0f;
        float currentVelocity = initialSpeed / displacement.magnitude;
        while (current < 1.0f)
        {
            float next = Mathf.SmoothDamp(current, 1.0f, ref currentVelocity, duration);
            transform.Translate((next - current) * displacement);
            current = next;
            yield return null;
        }
        Debug.Log("Forced Movement Done.");
    }
}
