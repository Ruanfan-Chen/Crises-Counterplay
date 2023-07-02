using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveItem_2 : ActiveItem
{
    private GameObject view;
    private ViewBehavior viewScript;
    private CircleCollider2D viewTrigger;
    private float viewRadius = 10.0f;
    private float initialDashSpeed = 50.0f;
    private float dashDistance = 5.0f;
    private float dashDuration = 0.5f;
    private float timer = 0.0f;
    private float cooldown = 3.0f;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        GameObject cooldown = GameObject.Find("Cooldown");
        if (cooldown != null)
        {
            timerText = cooldown.GetComponent<TextMeshProUGUI>();
        }
    }

    void OnEnable()
    {
        view = new GameObject("DashView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        viewScript = view.AddComponent<ViewBehavior>();
        viewScript.SetRepelVehicle(false);
        viewTrigger = view.AddComponent<CircleCollider2D>();
        viewTrigger.radius = viewRadius;
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        float val = Mathf.Clamp(timer, 0.0f, cooldown);
        timerText.text = Mathf.Round(val).ToString() + "s";
    }
    public override void Activate()
    {
        List<GameObject> targets = viewScript.GetCurrentCollisions();
        if (targets.Count > 0 && timer <= 0.0f)
        {
            StartCoroutine(Utility.AddAndRemoveComponent(gameObject, typeof(Invulnerable), dashDuration));
            StartCoroutine(RepelVehicles(dashDuration));
            StartCoroutine(Utility.ForcedMovement(transform.parent, (targets[0].transform.position - transform.position).normalized * dashDistance, initialDashSpeed, dashDuration));
            timer = cooldown;
        }
    }

    private IEnumerator RepelVehicles(float duration)
    {
        viewScript.SetRepelVehicle(true);
        yield return new WaitForSeconds(duration);
        viewScript.SetRepelVehicle(false);
    }

    private class ViewBehavior : MonoBehaviour
    {
        private List<GameObject> currentCollisions = new();
        private bool repelVehicle;

        public bool GetRepelVehicle() { return repelVehicle; }

        public void SetRepelVehicle(bool value) { repelVehicle = value; }

        public List<GameObject> GetCurrentCollisions() { return currentCollisions; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Vehicle>() != null)
                currentCollisions.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            currentCollisions.Remove(collision.gameObject);
        }

        void Update()
        {
            if (repelVehicle)
            {
                foreach (GameObject vehicle in currentCollisions)
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
