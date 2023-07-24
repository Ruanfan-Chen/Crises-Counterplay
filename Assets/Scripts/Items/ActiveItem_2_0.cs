using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

public class ActiveItem_2_0 : ActiveItem
{
    private static readonly string itemName = "Gravity Grasp";
    private static readonly string description = "Hold activation key to capture nearby trains. Trains will rotate around the character, dealing damage to the enemies. Controling more trains costs more enegy.";
    private static readonly string usage = "Active: Hold K";
    private static readonly string logoPath = "Sprites/Skills/Chistrike";
    private static readonly string notUsablePath = "Sprites/Skills/Chistrike";
    private readonly float viewRadius = 10.0f;
    private readonly float maxCharge = 5.0f;
    private readonly float costRate = 1.0f;
    private readonly List<Captured> scripts = new();
    private readonly float orbitRadius = 5.0f;
    private readonly float capturedSpeed = 35.0f;
    private GameObject view;
    private float charge;

    void Start()
    {
        ResetCharge();
    }

    public override void ResetCharge()
    {
        charge = 5.0f;
    }

    void OnEnable()
    {
        view = new GameObject("CaptureView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        CircleCollider2D viewTrigger = view.AddComponent<CircleCollider2D>();
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

    public override float GetChargeProgress() => charge / maxCharge;

    public override bool IsUsable() => charge > 0.0f && OverlapVehicle().Count() > 0;

    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public override Sprite GetUISprite() => IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);

    public static GameObject GetShopOption()
    {
        GameObject shopOption = ShopOption.Instantiate();
        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(GetLogo());
        script.SetItemName(GetName());
        script.SetUsage(GetUsage());
        script.SetDescription(GetDescription());
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_2_0>(KeyCode.K);
        });
        return shopOption;
    }

    private IEnumerable<GameObject> OverlapVehicle() => OverlapGameObject(view, collider => collider.GetComponent<Vehicle>());

    private void CaptureNearbyVehicles()
    {
        foreach (GameObject vehicle in OverlapVehicle())
        {
            if (vehicle.GetComponent<Vehicle>().GetHostility())
            {
                vehicle.GetComponent<Vehicle>().SetHostility(false);
                vehicle.GetComponent<Vehicle>().SetSpeed(capturedSpeed);
                vehicle.AddComponent<Captured>();
                vehicle.GetComponent<Captured>().SetCenter(gameObject);
                vehicle.GetComponent<Captured>().SetOrbitRadius(orbitRadius);
                scripts.Add(vehicle.GetComponent<Captured>());
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