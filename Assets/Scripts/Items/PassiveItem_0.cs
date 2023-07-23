using UnityEngine;
using static Utility;
public class PassiveItem_0 : PassiveItem
{
    private static readonly string itemName = "Toxic Footprint";
    private static readonly string description = "Character leaves a trail of toxic footprints. Footprint fades over time and deals damage to enemies in contact.";
    private static readonly string usage = "Passive";
    private static readonly string logoPath = "Sprites/Items/Toxic Footprint";
    private readonly float width = 1.0f;
    private readonly float trailDuration = 5.0f;
    private GameObject trailObj;
    private Color color = Color.green;


    private void OnEnable()
    {
        trailObj = new GameObject(itemName + "Trail");
        trailObj.transform.parent = transform;
        trailObj.transform.localPosition = Vector3.forward * MapManager.MAP_DEPTH / 2.0f;
        trailObj.transform.localRotation = Quaternion.identity;
        trailObj.transform.localScale = Vector3.one;
        TrailRenderer trailRenderer = trailObj.AddComponent<TrailRenderer>();
        trailRenderer.time = trailDuration;
        trailRenderer.numCapVertices = 8;
        trailRenderer.material = new Material(Shader.Find(DEAFULT_LINE_SHADER_PATH));
        trailRenderer.startColor = color;
        trailRenderer.startWidth = width;
        trailRenderer.endColor = new Color(color.r, color.g, color.b, 0.5f);
        trailRenderer.endWidth = width;
        ColliderManager.AddSharedCollider<Foorprint>(trailRenderer);
    }
    private void OnDisable()
    {
        Destroy(trailObj);
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
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_0>();
        });
        return shopOption;
    }

    private class Foorprint:MonoBehaviour
    {
        private float contactDPS = 25.0f;
        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && !collision.GetComponent<Character>())
                new Damage(gameObject, null, damageable, contactDPS * Time.deltaTime, Vector3.zero).Apply();
        }
    }
}