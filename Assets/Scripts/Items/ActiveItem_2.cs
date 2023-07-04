using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_2 : ActiveItem
{
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Resources/Placeholder";
    private GameObject view;
    private ViewBehavior viewScript;
    private CircleCollider2D viewTrigger;
    private float viewRadius = 10.0f;
    private float initialDashSpeed = 50.0f;
    private float dashDistance = 5.0f;
    private float dashDuration = 0.5f;
    private float charge = 3.0f;
    private float cost = 3.0f;
    //[SerializeField] private TextMeshProUGUI timerText;

    //private void Start()
    //{
    //    //GameObject cooldown = GameObject.Find("Cooldown");
    //    //if (cooldown != null)
    //    //{
    //    //    timerText = cooldown.GetComponent<TextMeshProUGUI>();
    //    //}
    //}

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
            StartCoroutine(Utility.AddAndRemoveComponent<Invulnerable>(gameObject, dashDuration));
            StartCoroutine(RepelVehicles(dashDuration));
            StartCoroutine(Utility.ForcedMovement(transform.parent, (viewScript.GetCurrentCollisions()[0].transform.position - transform.position).normalized * dashDistance, initialDashSpeed, dashDuration));
            charge -= cost;
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
        return charge >= cost && viewScript.GetCurrentCollisions().Count > 0;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public override string GetName()
    {
        return itemName;
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
