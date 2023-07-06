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
        if (character) {
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
    }
}
