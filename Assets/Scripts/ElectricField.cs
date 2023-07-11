using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ElectricField : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ElectricField";
    private static float forkedLightningSearchRadius = 5.0f;
    private static float forkedLightningDuration = 3.0f;

    private float damage;

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

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

    public static IEnumerator Instantiate(Vector3 position, float traceDuration, float delay, float radius, float electricFieldDuration, float damage)
    {
        Destroy(DrawCircle("ElectricField", position, radius, Color.yellow), traceDuration);
        yield return new WaitForSeconds(delay);
        GameObject electricField = Instantiate(Resources.Load<GameObject>(prefabPath), position, Quaternion.identity);
        electricField.transform.localScale = new Vector3(radius * 2.0f, radius * 2.0f, 1.0f);
        electricField.tag = "Disposable";
        ElectricField driverScript = electricField.GetComponent<ElectricField>();
        driverScript.SetDamage(damage);
        DestroyOutOfTime destroyScript = electricField.GetComponent<DestroyOutOfTime>();
        destroyScript.SetTimer(electricFieldDuration);
        destroyScript.Activate();

        PhysicsShapeGroup2D shapeGroup = ColliderManager.GetShapeGroup<Waterblight.WaterLayer>();
        for (int shapeIndex = 0; shapeIndex < shapeGroup.shapeCount; shapeIndex++)
        {
            PhysicsShape2D shape = shapeGroup.GetShape(shapeIndex);
            for (int vertexIndex = 0; vertexIndex < shape.vertexCount; vertexIndex++)
            {
                Vector2 vertex = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex);
                if ((vertex - (Vector2)position).magnitude <= forkedLightningSearchRadius)
                {
                    List<Vector2> vertices = new();
                    shapeGroup.GetShapeVertices(shapeIndex, vertices);
                    GameObject lineObj1 = DrawLine("ForkedLightning", position, vertex, Color.yellow);
                    GameObject lineObj2 = DrawLine("ForkedLightning", vertices, false, Color.yellow);
                    lineObj1.AddComponent<ElectricField>();
                    lineObj2.AddComponent<ElectricField>();
                    lineObj1.AddComponent<EdgeCollider2D>().SetPoints(new List<Vector2>() { position, vertex });
                    lineObj2.AddComponent<EdgeCollider2D>().SetPoints(vertices);
                    Destroy(lineObj1, forkedLightningDuration);
                    Destroy(lineObj2, forkedLightningDuration);
                    break;
                }
            }
        }
    }
}
