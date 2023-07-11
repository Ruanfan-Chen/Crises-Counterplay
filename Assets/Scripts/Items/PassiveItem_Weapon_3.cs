using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

public class PassiveItem_Weapon_3 : PassiveItem, IWeapon
{
    private static string prefabPath = "Prefabs/Footprint";
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Resources/Placeholder";
    private GameObject projectile;
    private ProjectileBehavior projectilScript;
    private GameObject view;
    private PolygonCollider2D viewTrigger;
    private float range = 10.0f;
    private float angleOfView = 120.0f;
    private int interpolationDensity = 4;

    public float GetDamage() { return projectilScript.GetContactDPS(); }
    public void SetDamage(float value) { projectilScript.SetContactDPS(value); }

    public float GetRange() { return range; }

    public void SetRange(float value) { range = value; UpdateCollider(); }

    public float GetProjectileSpeed() { return projectilScript.GetSpeed(); }

    public void SetProjectileSpeed(float value) { projectilScript.SetSpeed(value); }

    public float GetAngleOfView() { return angleOfView; }

    public void SetAngleOfView(float value) { angleOfView = value; UpdateCollider(); }

    public float GetAttackInterval() { return float.NaN; }

    public void SetAttackInterval(float value) { }

    private void OnEnable()
    {
        view = new GameObject("WeaponView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        viewTrigger = view.AddComponent<PolygonCollider2D>();
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
        UpdateCollider();
        projectile = Instantiate(Resources.Load<GameObject>(prefabPath), transform.position, transform.rotation);
        projectile.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        projectilScript = projectile.AddComponent<ProjectileBehavior>();
        projectilScript.SetController(this);
        projectilScript.SetHostility(GetComponent<Character>().GetHostility());
        projectilScript.SetTarget(gameObject);
    }

    private void OnDisable()
    {
        Destroy(projectile);
        view = null;
        viewTrigger = null;
        projectile = null;
        projectilScript = null;
    }

    private void UpdateCollider()
    {
        List<Vector2> points = new();
        points.Add(Vector2.zero);
        for (float i = 0; i <= interpolationDensity; i++)
        {
            points.Add(range * (Quaternion.Euler(0, 0, (i / interpolationDensity - 0.5f) * angleOfView) * Vector3.up));
        }
        viewTrigger.SetPath(0, points);
    }

    public void UpdateTarget()
    {
        if (projectilScript.GetTarget() != gameObject)
        {
            projectilScript.SetTarget(gameObject);
            return;
        }
        IEnumerable<GameObject> others = OverlapDamageable().Where(damageable => damageable != gameObject);
        if (others.Count() > 0)
            projectilScript.SetTarget(others.ElementAt(Random.Range(0, others.Count())));
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

    private IEnumerable<GameObject> OverlapDamageable()
    {
        return OverlapGameObject(view, collider => collider.GetComponent<IDamageable>() != null);
    }
    private class ProjectileBehavior : MonoBehaviour
    {
        private GameObject target;
        private float speed = 7.5f;
        private PassiveItem_Weapon_3 controller;
        private float contactDPS = 100.0f;
        private bool hostility;

        public PassiveItem_Weapon_3 GetController() { return controller; }

        public void SetController(PassiveItem_Weapon_3 value) { controller = value; }

        public float GetSpeed() { return speed; }

        public void SetSpeed(float value) { speed = value; }

        public GameObject GetTarget() { return target; }

        public void SetTarget(GameObject value) { target = value; }

        public float GetContactDPS() { return contactDPS; }

        public void SetContactDPS(float value) { contactDPS = value; }

        public bool GetHostility() { return hostility; }

        public void SetHostility(bool value) { hostility = value; }

        private void Update()
        {
            if (target == null)
            {
                controller.UpdateTarget();
                return;
            }
            Vector3 displacement = target.transform.position - transform.position;
            float speed = GetSpeed();
            if (speed * Time.deltaTime < displacement.magnitude)
                transform.Translate(speed * Time.deltaTime * displacement.normalized);
            else
            {
                transform.Translate(displacement);
                controller.UpdateTarget();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != hostility)
            {
                new Damage(controller.gameObject, gameObject, damageable, contactDPS * Time.deltaTime).Apply();
            }
        }
    }
}
