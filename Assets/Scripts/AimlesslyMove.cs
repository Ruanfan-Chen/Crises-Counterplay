using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimlesslyMove : MonoBehaviour
{
    private float timer;
    private Vector3 direction;
    private float speed = 2.5f;
    private float minHaltTime = 0.5f;
    private float maxHaltTime = 1.5f;
    private float minMoveDistance = 2.5f;
    private float maxMoveDistance = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        Halt(0);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        transform.Translate(speed * Time.deltaTime * direction);
        if (timer <= 0)
        {
            if (direction.magnitude == 0)
            {
                float theta = Random.Range(0.0f, 2 * Mathf.PI);
                Move(Random.Range(minMoveDistance, maxMoveDistance) * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
            }
            else
                Halt(Random.Range(minHaltTime, maxHaltTime));
        }
    }
    void Halt(float seconds)
    {
        timer = seconds;
        direction = Vector3.zero;
    }

    void Move(Vector3 displacement)
    {
        timer = displacement.magnitude / speed;
        direction = displacement.normalized;
    }
}
