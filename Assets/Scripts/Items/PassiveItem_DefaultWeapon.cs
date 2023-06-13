using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_DefaultWeapon : PassiveItem
{
    private GameObject view;
    private viewBehavior viewScript;
    private PolygonCollider2D viewTrigger;
    private float range;
    private float angleOfView;
    private float attackInterval;
    private int interpolationDensity = 4;

    void Start()
    {
        view = new GameObject("DefaultWeaponView");
        view.transform.SetParent(gameObject.transform);
        view.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
        viewScript = view.AddComponent<viewBehavior>();
        viewTrigger = view.AddComponent<PolygonCollider2D>();
        viewTrigger.isTrigger = true;
        view.AddComponent<Rigidbody2D>().isKinematic = true;
        UpdateCollider(0.0f, 0.0f);
        UpdateAttackInterval(0.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Character character = GetComponent<Character>();
        float newRange = character.GetRange();
        float newAngleOfView = character.GetAngleOfView();
        float newAttackInterval = character.GetAttackInterval();
        if (range != newRange || angleOfView != newAngleOfView)
        {
            UpdateCollider(newRange, newAngleOfView);
        }
        if (attackInterval != newAttackInterval)
            UpdateAttackInterval(newAttackInterval);
    }

    private void UpdateCollider(float newRange, float newAngleOfView)
    {
        List<Vector2> points = new();
        points.Add(Vector2.zero);
        for (float i = 0; i <= interpolationDensity; i++)
        {
            points.Add(newRange * (Quaternion.Euler(0, 0, (i / interpolationDensity - 0.5f) * newAngleOfView) * Vector3.up));
        }
        viewTrigger.SetPath(0, points);
        range = newRange;
        angleOfView = newAngleOfView;
    }

    private void UpdateAttackInterval(float newAttackInterval)
    {
        viewScript.SetAttackInterval(newAttackInterval);
        attackInterval = newAttackInterval;
    }

    private void OnDestroy()
    {
        Destroy(view);
    }

    // Script attached to View, a child of Character
    private class viewBehavior : MonoBehaviour
    {
        private float timer = 0.0f;
        private float attackInterval;

        public float GetAttackInterval() { return attackInterval; }

        internal void SetAttackInterval(float value) { attackInterval = value; }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (timer > 0) return;
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
