using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
    // Start is called before the first frame update

    private static string prefabPath = "Prefabs/Vehicle";
    public float speed = 15.0f;
    public static float traceLength = 1000.0f;
    public static float traceDuration = 2.0f;
    private Vector3 targetPos;
    private float contactDPS = 200.0f;
    private bool hostility = true;

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    void Start()
    {
        gameObject.tag = "Disposable";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = (targetPos - transform.position);
        Vector3 moveDir = displacement.normalized;
        if (displacement.magnitude >= speed * Time.deltaTime)
            // transform.position += moveDir * speed * Time.deltaTime;
            transform.Translate(speed * Time.deltaTime * Vector3.up);
        else
            Destroy(gameObject);
    }

    public static GameObject Instantiate()
    {
        // randomly selects pos on the map edge
        float x = 0.0f;
        float y = 0.0f;
        System.Random rnd = new System.Random();
        int n = rnd.Next(1, 8);
        //Debug.Log("n = " + n);
        // select starting from x = +/-60 or y = +/-30
        if (n % 2 == 1)
        {
            x = n % 4 == 1 ? -60.0f : 60.0f;
            y = Random.Range(-30, 30);
        }
        else
        {
            x = Random.Range(-60, 60);
            y = n % 4 == 2 ? -30.0f : 30.0f;
        }
        Vector3 startPos = new Vector3(x, y, 0);
        // randomly selects moving direction based on startPos, but prevents it from moving outwards when it starts
        float targetX = Random.Range(-60, 60);
        float targetY = Random.Range(-30, 30);
        float dirX = targetX - x;
        float dirY = targetY - y;
        if (Math.Abs(x) == 60)
        {
            dirX = Math.Abs(dirX) * Math.Sign(x) * (-1);
            targetX = x + dirX;
        }
        if (Math.Abs(y) == 30)
        {
            dirY = Math.Abs(dirY) * Math.Sign(y) * (-1);
            targetY = y + dirY;
        }
        Vector3 targetPos = new Vector3(targetX, targetY, 0);
        Vector3 dir = new Vector3(dirX, dirY, 0);
        //Debug.Log("dirX =" + dirX + ", dirY =" + dirY);
        GameObject vehicle = Instantiate(Resources.Load<GameObject>(prefabPath), startPos, Quaternion.LookRotation(Vector3.forward, dir));

        float width = vehicle.transform.localScale.x;

        Vehicle.drawTraces(x, y, dir, width);
        Vehicle script = vehicle.AddComponent<Vehicle>();
        script.SetTargetPos(targetPos);
        return vehicle;
    }

    private static void drawTraces(float startX, float startY, Vector3 dirVector, float vehicleWidth)
    {
        float x1, y1, x2, y2;
        if (Math.Abs(startX) == 60)
        {
            x1 = x2 = startX;
            y1 = Math.Min(30, startY + vehicleWidth / 2);
            y2 = Math.Max(-30, startY - vehicleWidth / 2);
        }
        else
        {
            y1 = y2 = startY;
            x1 = Math.Min(60, startX + vehicleWidth / 2);
            x2 = Math.Max(-60, startX - vehicleWidth / 2);
        }
        //Debug.Log("x1 = " + x1 + ", y1 = " + y1 + ", x2 = " + x2 + ", y2 = " + y2);
        Vector3 traceStartVector1 = new Vector3(x1, y1, 0);
        Vector3 traceEndVector1 = traceStartVector1 + dirVector * Vehicle.traceLength;
        Vector3 traceStartVector2 = new Vector3(x2, y2, 0);
        Vector3 traceEndVector2 = traceStartVector2 + dirVector * Vehicle.traceLength;

        LineDrawer lineDrawer1 = new LineDrawer();
        lineDrawer1.DrawLineInGameView(traceStartVector1, traceEndVector1, Color.green);
        lineDrawer1.Destroy(traceDuration);
        //Debug.Log("Drew lines from p1 = " + traceStartVector1);

        LineDrawer lineDrawer2 = new LineDrawer();
        lineDrawer2.DrawLineInGameView(traceStartVector2, traceEndVector2, Color.green);
        lineDrawer2.Destroy(traceDuration);
        //Debug.Log("Drew lines from p2 = " + traceStartVector2);

        // lineDrawer.DrawLineInGameView(traceStartVector2, traceEndVector2, Color.green);

        // Debug.DrawLine(traceStartVector1, traceEndVector1, Color.green, Vehicle.traceDuration);
        // Debug.DrawLine(traceStartVector2, traceEndVector2, Color.green, Vehicle.traceDuration);

    }

    public void SetTargetPos(Vector3 value)
    {
        targetPos = value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable.GetHostility() != hostility)
        {
            new Damage(gameObject, null, damageable, contactDPS * Time.deltaTime).Apply();
        }
    }
}
