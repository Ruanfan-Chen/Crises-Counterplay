using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAnim : MonoBehaviour
{
    private float duration = 1.0f;
    private float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += speed * Time.deltaTime * Vector3.up;
        Color color = GetComponent<SpriteRenderer>().color;
        float alpha = color.a - Time.deltaTime / duration;
        if (alpha > 0.0f)
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);
        else
            Destroy(gameObject);
    }
}
