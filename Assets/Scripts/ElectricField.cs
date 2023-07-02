using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricField : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ElectricField";

    private GameObject character;
    private float radius;
    private float damage;

    public GameObject GetCharacter() { return character; }

    public void SetCharacter(GameObject value) { character = value; }

    public float GetRadius() { return radius; }

    public void SetRadius(float value)
    {
        radius = value;
        transform.localScale = new Vector3(value, value, 1.0f);
    }

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

    void Update()
    {

    }

    private float GetDistance()
    {
        float rawDistance = (character.transform.position - transform.position).magnitude - radius;
        float characterRadius = character.GetComponent<CircleCollider2D>().radius;
        return rawDistance - Mathf.Clamp(rawDistance, -characterRadius, characterRadius);
    }
    public static GameObject Instantiate(GameObject character, Vector3 position, float radius, float duration, float damage)
    {
        GameObject electricField = Instantiate(Resources.Load<GameObject>(prefabPath), position, Quaternion.identity);
        ElectricField driverScript = electricField.GetComponent<ElectricField>();
        driverScript.SetCharacter(character);
        driverScript.SetRadius(radius);
        driverScript.SetDamage(damage);
        DestroyOutOfTime destroyScript = electricField.GetComponent<DestroyOutOfTime>();
        destroyScript.SetTimer(duration);
        destroyScript.Activate();
        return electricField;
    }
}
