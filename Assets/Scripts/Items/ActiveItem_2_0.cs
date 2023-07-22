using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static Utility;

public class ActiveItem_2_0 : ActiveItem
{
    private static string itemName = "Chistrike";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Chistrike";
    private static string tutorialPath = "Sprites/Tutorial/Tutorial_Chistrike";
    private static string notUsablePath = "Sprites/Skills/SkillsNotUsable";
    private GameObject view;
    private CircleCollider2D viewTrigger;
    private float viewRadius = 10.0f;
    private float maxCharge = 5.0f;
    private float charge = 5.0f;
    private float costRate = 1.0f;
    private List<Captured> scripts = new();
    private float orbitRadius = 5.0f;

    void OnEnable()
    {
        view = new GameObject("CaptureView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        viewTrigger = view.AddComponent<CircleCollider2D>();
        viewTrigger.radius = viewRadius;
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
    }
    void Update()
    {
        int i = scripts.Count;
        if (i == 0)
            charge = Mathf.Clamp(charge + Time.deltaTime, 0.0f, maxCharge);
        else
        {
            charge = Mathf.Clamp(charge - i * costRate * Time.deltaTime, 0.0f, maxCharge);
            if (charge <= 0.0f)
                Deactivate();
            CaptureNearbyVehicles();
        }
    }
    public override void Activate()
    {
        if (IsUsable())
        {
            CaptureNearbyVehicles();
        }
    }

    public override void Deactivate()
    {
        foreach (Captured script in scripts)
            Destroy(script);
        scripts.Clear();
    }

    public override float GetChargeProgress()
    {
        return charge / maxCharge;
    }

    public override bool IsUsable()
    {
        return charge > 0.0f && OverlapVehicle().Count() > 0;
    }

    public static string GetDescription()
    {
        return description;
    }

    public static Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public static string GetName()
    {
        return itemName;
    }

    public static Sprite GetTutorial()
    {
        return Resources.Load<Sprite>(tutorialPath);
    }

    public override Sprite GetUISprite()
    {
        return IsUsable() ? GetLogo() : GetLogo();
    }

    private IEnumerable<GameObject> OverlapVehicle()
    {
        return OverlapGameObject(view, collider => collider.GetComponent<Vehicle>());
    }

    private void CaptureNearbyVehicles()
    {
        foreach (GameObject vehicle in OverlapVehicle())
        {
            if (vehicle.GetComponent<Vehicle>().GetHostility())
            {
                vehicle.GetComponent<Vehicle>().SetHostility(false);
                Captured script = vehicle.AddComponent<Captured>();
                script.SetCenter(gameObject);
                script.SetOrbitRadius(orbitRadius);
                scripts.Add(script);
            }
        };
    }

    private class Captured : MonoBehaviour
    {
        private GameObject center;
        private float orbitRadius;

        public float GetOrbitRadius() { return orbitRadius; }

        public void SetOrbitRadius(float value) { orbitRadius = value; }

        public GameObject GetCenter() { return center; }

        public void SetCenter(GameObject value) { center = value; }

        void Update()
        {
            Vector3 relativePos = transform.position - center.transform.position;
            if (Vector3.Cross(relativePos, transform.rotation * Vector3.up).z >= 0.0f)
                transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 2 * Mathf.Atan2(relativePos.magnitude, orbitRadius) * Mathf.Rad2Deg) * relativePos.normalized);
            else
                transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, -2 * Mathf.Atan2(relativePos.magnitude, orbitRadius) * Mathf.Rad2Deg) * relativePos.normalized);
        }
    }
}