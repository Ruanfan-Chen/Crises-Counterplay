using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRelativelyStatic : MonoBehaviour
{
    private Transform reference;
    private Vector3 bias;

    public Transform GetReference() { return reference; }

    public void SetReference(Transform value) { reference = value; }

    public void SetBias() { bias = transform.position - reference.position; }

    void Update()
    {
        if (reference == null)
            Destroy(this);
        else
            transform.position = reference.position + bias;
    }
}
