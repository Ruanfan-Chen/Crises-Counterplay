using UnityEngine;
using static Utility;

public class ElectricArc : MonoBehaviour
{
    public static readonly float maxLength = 5.0f;
    public static readonly float breakBuffer = 0.5f;
    private GameObject vehicle;
    private ElectricField electricField;

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
                return;
            }
        }
        Destroy(gameObject);
    }

    public static GameObject Instantiate(GameObject vehicle, ElectricField electricField)
    {
        GetEndpoints(vehicle, electricField, out Vector3 startPos, out Vector3 endPos);
        GameObject electricArcObj = DrawLine("ElectricArc", startPos, endPos, Color.yellow);
        ElectricArc script = electricArcObj.AddComponent<ElectricArc>();
        script.vehicle = vehicle;
        script.electricField = electricField;
        return electricArcObj;
    }

    public static void GetEndpoints(GameObject vehicle, ElectricField electricField, out Vector3 startPos, out Vector3 endPos)
    {
        endPos = vehicle.GetComponent<Collider2D>().ClosestPoint(electricField.transform.position);
        startPos = electricField.transform.position + (endPos - electricField.transform.position).normalized * electricField.GetRadius();
    }
}
