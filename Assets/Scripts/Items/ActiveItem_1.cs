using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveItem_1 : ActiveItem
{
    private static string itemName = "EbbTide";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Ebbtide";
    private static string tutorialPath = "Sprites/Tutorial/Tutorial_EbbTide";
    private static string notUsablePath = "Sprites/Skills/Ebbtide";

    private float speed = 10.0f;
    public override void Activate()
    {
        if (IsUsable())
        {
            List<Vector3> positions = GetComponent<Waterblight>().GetTrail();
            StartCoroutine(Back(positions));
        }
    }

    private IEnumerator Back(List<Vector3> positions)
    {
        Buff buff = gameObject.AddComponent<Buff>();
        for (int i = positions.Count - 1; i > 0; i--)
        {
            Vector3 displacement = positions[i - 1] - positions[i];
            transform.position = positions[i];
            yield return new WaitForSeconds(displacement.magnitude / speed);
        }
        Destroy(buff);
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        return 1.0f;
    }

    public override Sprite GetUISprite()
    {
        return IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);
    }

    public override bool IsUsable()
    {
        return GetComponent<Waterblight>() != null;
    }

    public static string GetDescription()
    {
        return description;
    }

    public static Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public static string GetName()
    {
        return itemName;
    }

    public static Sprite GetTutorial()
    {
        return Resources.Load<Sprite>(tutorialPath);
    }

    public class Buff : MonoBehaviour, IInvulnerable, ISpeedBonus
    {
        private float damage = 50.0f;
        private float speedBonus = 5.0f;
        private Color colorDifference;

        private void OnEnable()
        {
            colorDifference = Color.yellow - GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        private void OnDisable()
        {
            GetComponent<SpriteRenderer>().color -= colorDifference;
        }

        public float GetValue()
        {
            return speedBonus;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != GetComponent<Character>().GetHostility())
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                new Damage(gameObject, null, damageable, damage, direction).Apply();
            }
        }
    }
}
