using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;
    private float xMin = float.MinValue;
    private float xMax = float.MaxValue;
    private float yMin = float.MinValue;
    private float yMax = float.MaxValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(speed
            * Time.deltaTime
            * (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput) ? Mathf.Abs(horizontalInput) : Mathf.Abs(verticalInput))
            * new Vector3(horizontalInput, verticalInput).normalized);
        if (transform.position.x < xMin)
            transform.position = new Vector3(xMin, transform.position.y, transform.position.z);
        if (transform.position.x > xMax)
            transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
        if (transform.position.y < yMin)
            transform.position = new Vector3(transform.position.x, yMin, transform.position.z);
        if (transform.position.y > yMax)
            transform.position = new Vector3(transform.position.x, yMax, transform.position.z);
    }
}
