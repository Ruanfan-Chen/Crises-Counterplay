using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_2 : ActiveItem
{
    private GameObject view;
    private ViewBehavior viewScript;
    private CircleCollider2D viewTrigger;
    private float viewRadius = 10.0f;
    private float dashSpeed = 50.0f;
    private float displacement = 10.0f;
    private float timer = 0.0f;
    private float cooldown = 8.0f;


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
    }
    public override void Activate()
    {
        List<GameObject> targets = viewScript.GetCurrentCollsions();
        if (targets.Count > 0 && timer <= 0.0f)
        {
            StartCoroutine(Dash(targets[0].transform.position));
            timer = cooldown;
        }
    }

    IEnumerator Dash(Vector3 targetPos)
    {
        float cumulativeDisplacement = 0.0f;
        Vector3 direction = (targetPos - transform.position).normalized;
        while (cumulativeDisplacement + dashSpeed * Time.deltaTime <= displacement)
        {
            cumulativeDisplacement += dashSpeed * Time.deltaTime;
            GetComponentInParent<Player>().transform.Translate(dashSpeed * Time.deltaTime * direction);
            yield return null;
        }
        GetComponentInParent<Player>().transform.Translate((displacement - cumulativeDisplacement) * direction);
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
