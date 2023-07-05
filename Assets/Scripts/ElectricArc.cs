using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Utility;

public class ElectricArc : MonoBehaviour
{
    public static readonly float maxLength = 5.0f;
    private GameObject vehicle;
    private ElectricField electricField;

    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Vector3 endpoint = GetEFEndpoint();
        lineRenderer.SetPosition(0, vehicle.GetComponent<Collider2D>().ClosestPoint(endpoint));
        lineRenderer.SetPosition(1, endpoint);
    }
    private Vector3 GetEFEndpoint()
    {
        return GetEFEndpoint(vehicle.transform.position, electricField.transform.position, electricField.GetRadius());
    }

    public static GameObject Instantiate(GameObject vehicle, ElectricField electricField)
    {
        GameObject electricArcObj = DrawLine("ElectricArc", vehicle.transform.position, GetEFEndpoint(vehicle.transform.position, electricField.transform.position, electricField.GetRadius()), Color.yellow);
        ElectricArc script = electricArcObj.AddComponent<ElectricArc>();
        script.vehicle = vehicle;
        script.electricField = electricField;
        return electricArcObj;
    }

    public static Vector3 GetEFEndpoint(Vector3 vehiclePos, Vector3 electricFieldCenter, float electricFieldRadius)
    {
        return electricFieldCenter + (vehiclePos - electricFieldCenter).normalized * electricFieldRadius;
    }
}
