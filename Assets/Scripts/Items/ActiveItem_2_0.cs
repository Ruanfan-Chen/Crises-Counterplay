using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveItem_2_0 : ActiveItem
{
    private GameObject view;
    private ViewBehavior viewScript;
    private CircleCollider2D viewTrigger;
    private float viewRadius = 10.0f;
    private float timer = 0.0f;
    private float cooldown = 3.0f;
    [SerializeField] private TextMeshProUGUI timerText;
    private float orbitRadius = 5.0f;

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
        view = new GameObject("CaptureView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        viewScript = view.AddComponent<ViewBehavior>();
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
        List<GameObject> targets = viewScript.GetCurrentCollsions();
        if (targets.Count > 0 && timer <= 0.0f)
        {
            foreach (GameObject vehicle in targets)
            {
                if (vehicle.activeInHierarchy && vehicle.GetComponent<Vehicle>().GetHostility() != GetComponent<Character>().GetHostility())
                {
                    vehicle.GetComponent<Vehicle>().SetHostility(false);
                    Captured script = vehicle.AddComponent<Captured>();
                    script.SetCenter(gameObject);
                    script.SetOrbitRadius(orbitRadius);
                }
            };
            timer = cooldown;
        }
    }

    public override float GetChargeProgress()
    {
        throw new System.NotImplementedException();
    }

    private class ViewBehavior : MonoBehaviour
    {
        private List<GameObject> currentCollsions = new();

        public List<GameObject> GetCurrentCollsions() { return currentCollsions; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Vehicle>() != null)
                currentCollsions.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            currentCollsions.Remove(collision.gameObject);
        }
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
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 2 * Mathf.Atan2(relativePos.magnitude, orbitRadius) * Mathf.Rad2Deg) * relativePos.normalized);
        }
    }
}