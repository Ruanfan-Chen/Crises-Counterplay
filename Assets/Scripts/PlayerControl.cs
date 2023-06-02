using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject map;
    private float offset = 2.0f;
    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 min;
    private Vector3 max;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        min = map.transform.position - map.transform.lossyScale / 2;
        max = map.transform.position + map.transform.lossyScale / 2;
        transform.Translate(speed
            * Time.deltaTime
            * (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput) ? Mathf.Abs(horizontalInput) : Mathf.Abs(verticalInput))
            * new Vector3(horizontalInput, verticalInput).normalized);
        if (transform.position.x < min.x + offset)
            transform.position = new Vector3(min.x + offset, transform.position.y, transform.position.z);
        if (transform.position.x > max.x - offset)
            transform.position = new Vector3(max.x - offset, transform.position.y, transform.position.z);
        if (transform.position.y < min.y + offset)
            transform.position = new Vector3(transform.position.x, min.y + offset, transform.position.z);
        if (transform.position.y > max.y - offset)
            transform.position = new Vector3(transform.position.x, max.y - offset, transform.position.z);
    }
}
