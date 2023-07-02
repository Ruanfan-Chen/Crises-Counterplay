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
            StartCoroutine(Dash((targets[0].transform.position - transform.position).normalized * dashDistance));
            timer = cooldown;
        }
    }

    IEnumerator Dash(Vector3 displacement)
    {
        return Utility.ForcedMovement(transform,displacement, initialDashSpeed, dashDuration);
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
}
