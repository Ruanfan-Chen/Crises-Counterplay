using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_1 : ActiveItem
{
    private static readonly string itemName = "Surf Mania";
    private static readonly string description = "Can be activated when having water trace, moving back along trace. Gains move speed bonus and deals damage to enemies colliding during activation.";
    private static readonly string usage = "Active: Press L";
    private static readonly string logoPath = "Sprites/Skills/Ebbtide";
    private static readonly string notUsablePath = "Sprites/Skills/Ebbtide";

    private readonly float speed = 10.0f;
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
        float s = 0.0f;
        for (int i = positions.Count - 1; i > 0; i--)
        {
            s += (positions[i - 1] - positions[i]).magnitude;
            transform.position += positions[i - 1] - positions[i];
            if (s > speed * Time.deltaTime)
            {
                s -= speed * Time.deltaTime;
                yield return null;
            }
        }
        Destroy(buff);
    }

    public override void Deactivate() { }

    public override float GetChargeProgress() => 1.0f;

    public override Sprite GetUISprite() => IsUsable() ? GetLogo() : Resources.Load<Sprite>(notUsablePath);

    public override bool IsUsable() => GetComponent<Waterblight>() != null && GetComponent<Buff>() == null;

    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public static GameObject GetShopOption()
    {
        GameObject shopOption = ShopOption.Instantiate();
        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(GetLogo());
        script.SetItemName(GetName());
        script.SetUsage(GetUsage());
        script.SetDescription(GetDescription());
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_1>(KeyCode.L);
        });
        return shopOption;
    }

    public class Buff : MonoBehaviour, IInvulnerable, ISpeedBonus, IMoveDisabled
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
