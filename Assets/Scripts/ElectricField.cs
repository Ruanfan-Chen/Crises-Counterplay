using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ElectricField : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ElectricField";

    private float damage;

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character)
        {
            Vector3 direction = ((Vector2)collision.transform.position - GetComponent<Collider2D>().ClosestPoint(collision.transform.position)).normalized;
            new Damage(gameObject, null, character, damage, direction).Apply();
        }
    }

    public static IEnumerator Instantiate(Vector3 position, float traceDuration, float delay, float radius, float electricFieldDuration, float damage)
    {
        Destroy(DrawCircle("ElectricField", position, radius, Color.yellow), traceDuration);
        bool batterySpawn = ActiveItem_0.GetBatterySpawn();
        if (batterySpawn)
            Destroy(Battery.InstantiateTrace(position, Quaternion.identity), traceDuration);
        yield return new WaitForSeconds(delay);
        if (batterySpawn)
            Destroy(Battery.Instantiate(position, Quaternion.identity), electricFieldDuration);
        GameObject electricField = Instantiate(Resources.Load<GameObject>(prefabPath), position + Vector3.forward * MapManager.MAP_DEPTH / 2.0f, Quaternion.identity);
        electricField.transform.localScale = new Vector3(radius * 2.0f, radius * 2.0f, 1.0f);
        electricField.tag = "Disposable";
        ElectricField driverScript = electricField.GetComponent<ElectricField>();
        driverScript.SetDamage(damage);
        Destroy(electricField, electricFieldDuration);

        PhysicsShapeGroup2D shapeGroup = ColliderManager.GetShapeGroup<Waterblight.WaterLayer>();
        for (int shapeIndex = 0; shapeIndex < shapeGroup.shapeCount; shapeIndex++)
        {
            PhysicsShape2D shape = shapeGroup.GetShape(shapeIndex);
            for (int vertexIndex = 0; vertexIndex < shape.vertexCount; vertexIndex++)
            {
                Vector2 vertex = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex);
                if ((vertex - (Vector2)position).magnitude <= 5.0f)
                {
                    List<Vector2> vertices = new();
                    shapeGroup.GetShapeVertices(shapeIndex, vertices);
                    GameObject lineObj1 = DrawLine("ForkedLightning", position, vertex, Color.yellow);
                    GameObject lineObj2 = DrawLine("ForkedLightning", vertices, false, Color.yellow);
                    lineObj1.AddComponent<ElectricField>().SetDamage(damage);
                    lineObj2.AddComponent<ElectricField>().SetDamage(damage);
                    lineObj1.AddComponent<EdgeCollider2D>().SetPoints(new List<Vector2>() { position, vertex });
                    lineObj2.AddComponent<EdgeCollider2D>().SetPoints(vertices);
                    Destroy(lineObj1, 1.0f);
                    Destroy(lineObj2, 1.0f);
                    break;
                }
            }
        }
    }
}
