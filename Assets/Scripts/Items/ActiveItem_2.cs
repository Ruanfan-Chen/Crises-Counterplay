using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

public class ActiveItem_2 : ActiveItem
{
    private static string itemName = "Trainbound";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Trainbound";
    private static string tutorialPath = "Sprites/Tutorial/Placeholder";
    private static string notUsablePath = "Sprites/Skills/Trainbound";
    public static int activateCounter = 0;
    private GameObject view;
    private ViewBehavior viewScript;
    private CircleCollider2D viewTrigger;
    private float viewRadius = 10.0f;
    private float initialDashSpeed = 50.0f;
    private float dashDistance = 5.0f;
    private float dashDuration = 0.5f;
    private float charge;
    private float cost = 3.0f;

    void Start()
    {
        ResetCharge();
    }

    public override void ResetCharge()
    {
        charge = 3.0f;
    }

    void OnEnable()
    {
        view = new GameObject("DashView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        viewScript = view.AddComponent<ViewBehavior>();
        viewScript.SetRepelVehicle(false);
        viewTrigger = view.AddComponent<CircleCollider2D>();
        viewTrigger.radius = viewRadius;
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
    }
    void Update()
    {
        charge = Mathf.Clamp(charge + Time.deltaTime, 0.0f, cost);
        //float val = Mathf.Clamp(timer, 0.0f, cooldown);
        //timerText.text = Mathf.Round(val).ToString() + "s";
    }
    public override void Activate()
    {
        if (IsUsable())
        {
            StartCoroutine(AddAndRemoveComponent<Invulnerable>(gameObject, dashDuration));
            StartCoroutine(RepelVehicles(dashDuration));
            StartCoroutine(ForcedMovement(transform, (OverlapVehicle().ElementAt(Random.Range(0, OverlapVehicle().Count())).transform.position - transform.position).normalized * dashDistance, initialDashSpeed, dashDuration));
            charge -= cost;
            activateCounter++;
        }
    }

    private IEnumerator RepelVehicles(float duration)
    {
        viewScript.SetRepelVehicle(true);
        yield return new WaitForSeconds(duration);
        viewScript.SetRepelVehicle(false);
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        return charge / cost;
    }

    public override bool IsUsable()
    {
        return charge >= cost && OverlapVehicle().Count() > 0;
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

    private IEnumerable<GameObject> OverlapVehicle() {
        return OverlapGameObject(view, collider => collider.GetComponent<Vehicle>());
    }

    public override Sprite GetUISprite()
    {
        return IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);
    }

    private class ViewBehavior : MonoBehaviour
    {
        private bool repelVehicle;

        public bool GetRepelVehicle() { return repelVehicle; }

        public void SetRepelVehicle(bool value) { repelVehicle = value; }

        void Update()
        {
            if (repelVehicle)
            {
                foreach (GameObject vehicle in OverlapGameObject(gameObject, collider=>collider.GetComponent<Vehicle>()))
                {
                    if (vehicle.GetComponent<Vehicle>().GetHostility())
                    {
                        vehicle.transform.rotation = Quaternion.LookRotation(Vector3.forward, vehicle.transform.position - transform.position);
                        vehicle.GetComponent<Vehicle>().SetHostility(false);
                    }
                };
            }
        }
    }
}
