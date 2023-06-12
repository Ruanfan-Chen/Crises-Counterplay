using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_DefaultWeapon : PassiveItem
{
    private GameObject view;
    private PolygonCollider2D viewTrigger;
    private float range;
    private float angleOfView;
    private int interpolationDensity = 4;

    void Start()
    {
        view = new GameObject("DefaultWeaponView");
        view.transform.SetParent(gameObject.transform);
        viewTrigger = view.AddComponent<PolygonCollider2D>();
        viewTrigger.isTrigger = true;
        UpdateCollider(0.0f, 0.0f);
        view.AddComponent<visionBehavior>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Character character = GetComponent<Character>();
        float newRange = character.GetRange();
        float newAngleOfView = character.GetAngleOfView();
        if (range != newRange || angleOfView != newAngleOfView)
        {
            UpdateCollider(newRange, newAngleOfView);
        }
    }

    private void UpdateCollider(float newRange, float newAngleOfView)
    {
        Quaternion orientation = transform.rotation;
        List<Vector2> points = new();
        points.Add(Vector2.zero);
        for (float i = 0; i <= interpolationDensity; i++)
        {
            points.Add(newRange * (Quaternion.Euler(0, 0, (i / interpolationDensity - 0.5f) * newAngleOfView) * Vector3.up));
        }
        viewTrigger.SetPath(0, points);
        range = newRange;
        angleOfView = newAngleOfView;
    }

    private void OnDestroy()
    {
        Destroy(view);
    }

    private class visionBehavior : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collision)
        {

        }
    }
}
