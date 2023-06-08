using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    private float speed;

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(GetSpeed() * Time.deltaTime * Vector3.up);
    }
}
