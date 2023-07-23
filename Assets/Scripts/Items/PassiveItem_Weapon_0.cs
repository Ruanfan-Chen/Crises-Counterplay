using UnityEngine;

public class PassiveItem_Weapon_0 : PassiveItem, IProjectileModifier
{
    private static readonly string itemName = "Handy Pistol";
    private static readonly string description = "Periodically fire a bullet to the closest enemy within the character's range.";
    private static readonly string usage = "Passive: Weapon";
    private static string logoPath = "Sprites/Items/Regular Gun";
    private GameObject view;
    private CircleCollider2D viewTrigger;
    private float damage = 25.0f;
    private float range = 10.0f;
    private float projectileSpeed = 20.0f;

    void OnEnable()
    {
        view = new GameObject("WeaponView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        view.AddComponent<ViewBehavior>();
        viewTrigger = view.AddComponent<CircleCollider2D>();
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
        viewTrigger.radius = range;
    }
    void OnDisable()
    {
        Destroy(view);
        view = null;
        viewTrigger = null;
    }
    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public static GameObject getShopOption()
    {
        GameObject shopOption = ShopOption.Instantiate();
        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(GetLogo());
        script.SetItemName(GetName());
        script.SetUsage(GetUsage());
        script.SetDescription(GetDescription());
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_Weapon_0>();
            UIManager.ClearShopPanel();
            GameplayManager.GetGoogleSender().SendMatrix4(GetName());
            GameplayManager.CloseShop();
        });
        return shopOption;
    }

    void IProjectileModifier.Modify(GameObject projectile)
    {
        projectile.AddComponent<Projectile.DamageOnCollision>().SetDamage(damage);
        projectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        if (float.IsFinite(range / projectileSpeed))
            Destroy(projectile, range / projectileSpeed);
    }

    private class ViewBehavior : MonoBehaviour
    {
        private float timer = 0.0f;
        private float attackInterval = 0.5f;

        public float GetAttackInterval() { return attackInterval; }

        public void SetAttackInterval(float value) { attackInterval = value; }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (timer > 0 || GetComponentInParent<IDisarmed>() != null) return;
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != GetComponentInParent<Character>().GetHostility())
            {
                Projectile.Instantiate(transform.position, collision.transform.position, GetComponentsInParent<IProjectileModifier>());
                timer = attackInterval;
            }
        }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
        }
    }
}
