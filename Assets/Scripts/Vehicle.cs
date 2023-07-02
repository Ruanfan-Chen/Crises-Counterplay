using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    // Start is called before the first frame update

    private static string prefabPath = "Prefabs/Vehicle";
    public float speed = 30.0f;
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
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    public static IEnumerator Instantiate(Vector3 startPos, Vector3 targetPos, float traceDuration, float vehicleDelay)
    {
        drawTraces(startPos, targetPos, Resources.Load<GameObject>(prefabPath).transform.lossyScale.x, traceDuration);
        yield return new WaitForSeconds(vehicleDelay);
        GameObject vehicle = Instantiate(Resources.Load<GameObject>(prefabPath), startPos, Quaternion.LookRotation(Vector3.forward, targetPos - startPos));
        vehicle.AddComponent<Vehicle>();
        vehicle.AddComponent<DestroyOutOfBounds>();
    }


    private static void drawTraces(Vector3 startPos, Vector3 targetPos, float vehicleWidth, float duration)
    {
        Vector3 bias = Quaternion.Euler(0, 0, 90) * (targetPos - startPos).normalized * vehicleWidth / 2;

        LineDrawer lineDrawer1 = new LineDrawer();
        lineDrawer1.DrawLineInGameView(startPos + bias, targetPos + bias, Color.green);
        lineDrawer1.Destroy(duration);
        //Debug.Log("Drew lines from p1 = " + traceStartVector1);

        LineDrawer lineDrawer2 = new LineDrawer();
        lineDrawer2.DrawLineInGameView(startPos - bias, targetPos - bias, Color.green);
        lineDrawer2.Destroy(duration);
        //Debug.Log("Drew lines from p2 = " + traceStartVector2);

        // lineDrawer.DrawLineInGameView(traceStartVector2, traceEndVector2, Color.green);

        // Debug.DrawLine(traceStartVector1, traceEndVector1, Color.green, Vehicle.traceDuration);
        // Debug.DrawLine(traceStartVector2, traceEndVector2, Color.green, Vehicle.traceDuration);

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
