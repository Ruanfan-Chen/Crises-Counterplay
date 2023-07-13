using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class ActiveItem_0 : ActiveItem
{
    private static string itemName = "Supercharge";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Supercharge";
    private float maxCharge = 5.0f;
    private float charge = 0.0f;
    private float cost = 5.0f;
    private float duration = 10.0f;

    void Update()
    {
        if (!GetComponent<Buff>() && GetComponent<Charger.Charging>())
            charge = Mathf.Clamp(charge + Time.deltaTime, 0.0f, maxCharge);
    }
    public override void Activate()
    {
        if (IsUsable())
        {
            StartCoroutine(AddAndRemoveComponent<Buff>(gameObject, duration));
            charge -= cost;
        }
    }

    public override void Deactivate() { }

    public override float GetChargeProgress()
    {
        return charge / maxCharge;
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

    public override bool IsUsable()
    {
        return charge >= cost;
    }

    private class Buff : MonoBehaviour, IInvulnerable, ISpeedBonus
    {
        private float damage = 50.0f;
        private float knockbackDistance = 3.0f;
        private float initialKnockbackSpeed = 10.0f;
        private float knockbackDuration = 1.0f;
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
            if (damageable != null)
            {
                Vector3 displacement = (collision.transform.position - transform.position).normalized * knockbackDistance;
                IEnumerator coroutine = ForcedMovement(collision.transform, displacement, initialKnockbackSpeed, knockbackDuration);
                damageable.StartCoroutine(coroutine);
                if (damageable.GetHostility() != GetComponent<Character>().GetHostility())
                    new Damage(gameObject, null, damageable, damage).Apply();
            }
        }
    }
}
