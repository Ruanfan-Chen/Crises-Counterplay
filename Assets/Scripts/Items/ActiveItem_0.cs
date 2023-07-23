using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ActiveItem_0 : ActiveItem
{
    private static readonly HashSet<ActiveItem_0> instances = new();
    private static string itemName = "Supercharge";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Supercharge";
    private static string tutorialPath = "Sprites/Tutorial/Tutorial_Supercharge";
    private static string notUsablePath = "Sprites/Skills/Supercharge";
    public static int activateCounter = 0;
    private int charge = 0;
    private float chargeBuffer = 0.0f;
    private float duration = 5.0f;
    private void OnEnable()
    {
        instances.Add(this);
    }

    private void OnDisable()
    {
        instances.Remove(this);
    }

    public static bool GetBatterySpawn() { return instances.Count > 0; }

    public override void Activate()
    {
        if (IsUsable())
        {
            StartCoroutine(AddAndRemoveComponent<Buff>(gameObject, duration));
            charge--;
            StartCoroutine(AddChargeBuffer(1.0f, duration));
            activateCounter++;
        }
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        return charge + chargeBuffer;
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

    public override bool IsUsable()
    {
        return charge > 0 && !GetComponent<Buff>();
    }

    public override Sprite GetUISprite()
    {
        return IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);
    }

    public void Charge()
    {
        charge++;
        StartCoroutine(AddChargeBuffer(-1.0f, 0.5f));
    }

    private IEnumerator AddChargeBuffer(float value, float duration)
    {
        chargeBuffer += value;
        float changeRate = value / duration;
        while (Mathf.Abs(value) > Mathf.Abs(changeRate) * Time.deltaTime)
        {
            value -= changeRate * Time.deltaTime;
            chargeBuffer -= changeRate * Time.deltaTime;
            yield return null;
        }
        chargeBuffer -= value;
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
