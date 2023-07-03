using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricField : MonoBehaviour
{
    private static string prefabPath = "Prefabs/ElectricField";

    private GameObject character;
    [SerializeField] private float radius;
    [SerializeField] private float damage;
    private bool m_isInRange = false;

    public GameObject GetCharacter() { return character; }

    public void SetCharacter(GameObject value) { character = value; }

    public float GetRadius() { return radius; }

    public void SetIsInRange(bool value) { m_isInRange = value; }

    public void SetRadius(float value)
    {
        radius = value;
        transform.localScale = new Vector3(value * 2, value * 2, 1.0f);
    }

    public float GetDamage() { return damage; }

    public void SetDamage(float value) { damage = value; }

    void Update()
    {
        float centerDistance = (character.transform.position - transform.position).magnitude;
        float characterRadius = character.GetComponent<CircleCollider2D>().radius;
        //if (radius - characterRadius < centerDistance && centerDistance < radius + characterRadius)
        //    new Damage(gameObject, null, character.GetComponent<IDamageable>(), damage).Apply();
        if (m_isInRange)
        {
            if(centerDistance > radius - characterRadius)
            {
                //push back to the field and do damage
            }
        }
        else
        {
            if(centerDistance < radius + characterRadius)
            {
                //push out and do damage
            }
        }
    }

    public static IEnumerator Instantiate(GameObject character, Vector3 position, float traceDuration, float delay, float radius, float electricFieldDuration, float damage)
    {
        foreach (LineDrawer lineDrawer in LineDrawer.DrawCircleInGameView(position, radius, Color.yellow))
        {
            lineDrawer.Destroy(traceDuration);
        }
        yield return new WaitForSeconds(delay);
        GameObject electricField = Instantiate(Resources.Load<GameObject>(prefabPath), position, Quaternion.identity);
        ElectricField driverScript = electricField.GetComponent<ElectricField>();
        driverScript.SetCharacter(character);
        driverScript.SetRadius(radius);
        driverScript.SetDamage(damage);

        float distance = (character.transform.position - position).magnitude;
        float characterRadius = character.GetComponent<CircleCollider2D>().radius;

        //in the field
        if (distance < radius - characterRadius)
        {
            driverScript.SetIsInRange(true);
        }
        //touching the field
        else if(distance < radius + characterRadius)
        {
            //push out to the field

            driverScript.SetIsInRange(false);
        }
        else
        {
            driverScript.SetIsInRange(false);
        }

        DestroyOutOfTime destroyScript = electricField.GetComponent<DestroyOutOfTime>();
        destroyScript.SetTimer(electricFieldDuration);
        destroyScript.Activate();
    }
}
