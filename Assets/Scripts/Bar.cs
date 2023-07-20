using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bar : MonoBehaviour
{
    [SerializeField] RectTransform barBackgroundRect;
    [SerializeField] RectTransform barTopRect;
    [SerializeField] float offset;
    [SerializeField] float value;
    [SerializeField] bool isHidden;

    public void SetIsHidden(bool value)
    {
        isHidden = value;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Clamp(value, 0.0f, 1.0f);
        barBackgroundRect.gameObject.SetActive(!isHidden);
        barTopRect.sizeDelta = new Vector2(value * (barBackgroundRect.rect.width - offset), barTopRect.sizeDelta.y);
    }
}
