using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimlesslyMove : MonoBehaviour
{
    private float timer;
    private Vector3 direction;
    private float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Stop(0);
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
                Move(Random.Range(1.0f, 3.0f) * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
            }
            else
                Stop(Random.Range(0.5f, 1.5f));
        }
    }
    void Stop(float seconds)
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
