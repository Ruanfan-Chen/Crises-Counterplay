using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private bool hostility;
    public float GetSpeed() { return speed; }

    public void SetSpeed(float value) { speed = value; }

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Disposable";
        GetComponent<DestroyOutOfBounds>().SetOffset(-15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(GetSpeed() * Time.deltaTime * Vector3.up);
    }
}
