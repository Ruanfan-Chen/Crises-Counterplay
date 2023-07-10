using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ElectricArc : MonoBehaviour
{
    public static readonly float maxLength = 5.0f;
    public static readonly float breakBuffer = 0.5f;
    private Collider2D vehicle;
    private Collider2D electricField;
    private float damage;

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (vehicle && electricField)
        {
            GetEndpoints(vehicle, electricField, out Vector3 startPos, out Vector3 endPos);
            if ((startPos - endPos).magnitude <= maxLength + breakBuffer)
            {
                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPos);
                GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2>() { startPos, endPos });
                return;
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character)
        {
            Vector3 displacement = ((Vector2)collision.transform.position - GetComponent<Collider2D>().ClosestPoint(collision.transform.position)).normalized * Character.knockbackDistanceOnDmg;
            IEnumerator coroutine = ForcedMovement(character.transform, displacement, Character.initialKnockbackSpeedOnDmg, Character.knockbackDurationOnDmg);
            new Damage(gameObject, null, character, damage, coroutine).Apply();
        }
    }

    public static GameObject Instantiate(Collider2D vehicle, Collider2D electricField, float damage)
    {
        GetEndpoints(vehicle, electricField, out Vector3 startPos, out Vector3 endPos);
        GameObject electricArcObj = DrawLine("ElectricArc", startPos, endPos, Color.yellow);
        electricArcObj.AddComponent<EdgeCollider2D>().SetPoints(new List<Vector2>() { startPos, endPos });
        ElectricArc script = electricArcObj.AddComponent<ElectricArc>();
        script.vehicle = vehicle;
        script.electricField = electricField;
        script.damage = damage;
        return electricArcObj;
    }

    public static void GetEndpoints(Collider2D vehicle, Collider2D electricField, out Vector3 startPos, out Vector3 endPos)
    {
        endPos = vehicle.ClosestPoint(electricField.transform.position);
        startPos = electricField.ClosestPoint(endPos);
    }
}
