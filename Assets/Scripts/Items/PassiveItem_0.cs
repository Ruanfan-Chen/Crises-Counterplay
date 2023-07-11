using UnityEngine;
using static Utility;
public class PassiveItem_0 : PassiveItem
{
    private static string itemName = "Toxic Footprint";
    private static string description = "Description Placeholder";
    private static string logoPath = "Sprites/Skills/Toxic Footprint";
    private GameObject trailObj;
    private Color color = Color.green;
    private float width = 1.0f;
    private float trailDuration = 5.0f;


    private void OnEnable()
    {
        trailObj = new GameObject(itemName + "Trail");
        trailObj.transform.parent = transform;
        trailObj.transform.localPosition = Vector3.forward * MapManager.MAP_DEPTH / 2.0f;
        trailObj.transform.localRotation = Quaternion.identity;
        trailObj.transform.localScale = Vector3.one;
        TrailRenderer trailRenderer = trailObj.AddComponent<TrailRenderer>();
        trailRenderer.tag = "Disposable";
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

    private class Foorprint:MonoBehaviour
    {
        private float contactDPS = 25.0f;
        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && !collision.GetComponent<Character>())
                new Damage(gameObject, null, damageable, contactDPS * Time.deltaTime).Apply();
        }
    }
}