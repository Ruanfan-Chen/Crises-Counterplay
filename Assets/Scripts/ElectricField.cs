using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ElectricField : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ElectricField";

    private GameObject character;
    private float radius;
    private float damage;
    private Dictionary<Collider2D, GameObject> currentArc = new();

    public GameObject GetCharacter() { return character; }

    public void SetCharacter(GameObject value) { character = value; }

    public float GetRadius() { return radius; }

    public void SetRadius(float value)
    {
        radius = value;
        transform.localScale = new Vector3(value * 2, value * 2, 1.0f);
        GetComponent<CircleCollider2D>().radius = 1 + ElectricArc.maxLength / value;
    }

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Vehicle>() != null)
            currentArc.Add(collision, ElectricArc.Instantiate(collision.gameObject, this));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentArc.TryGetValue(collision, out GameObject arc))
            Destroy(arc);
    }

    void Update()
    {
        Vector3 centerDisplacement = character.transform.position - transform.position;
        float characterRadius = character.GetComponent<CircleCollider2D>().radius;
        if (radius - characterRadius < centerDisplacement.magnitude && centerDisplacement.magnitude < radius + characterRadius)
        {
            Vector3 displacement = Mathf.Sign(centerDisplacement.magnitude - radius) * Character.knockbackDistanceOnDmg * centerDisplacement.normalized;
            IEnumerator coroutine = ForcedMovement(character.transform, displacement, Character.initialKnockbackSpeedOnDmg, Character.knockbackDurationOnDmg);
            new Damage(gameObject, null, character.GetComponent<IDamageable>(), damage, coroutine).Apply();
        }
    }

    public static IEnumerator Instantiate(GameObject character, Vector3 position, float traceDuration, float delay, float radius, float electricFieldDuration, float damage)
    {
        Destroy(DrawCircle("ElectricField", position, radius, Color.yellow), traceDuration);
        yield return new WaitForSeconds(delay);
        GameObject electricField = Instantiate(Resources.Load<GameObject>(prefabPath), position, Quaternion.identity);
        electricField.tag = "Disposable";
        ElectricField driverScript = electricField.GetComponent<ElectricField>();
        driverScript.SetCharacter(character);
        driverScript.SetRadius(radius);
        driverScript.SetDamage(damage);
        DestroyOutOfTime destroyScript = electricField.GetComponent<DestroyOutOfTime>();
        destroyScript.SetTimer(electricFieldDuration);
        destroyScript.Activate();
    }
}
