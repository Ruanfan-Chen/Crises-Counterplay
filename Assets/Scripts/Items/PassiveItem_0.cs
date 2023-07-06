using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PassiveItem_0 : PassiveItem
{
    private static string prefabPath = "Prefabs/Footprint";
    private static string itemName = "Name Placeholder";
    private static string description = "Description Placeholder";
    private static string logoPath = "Resources/Placeholder";
    private Vector3 prevPos;
    private float stepsize;
    private float minStepsize = 0.5f;
    private float maxStepsize = 3.0f;
    private float contactDPS = 25.0f;
    // maximun duration (if do 0 damage ro enemies)
    private float maxDuration = 10.0f;
    // maximun total Damage to enemies
    private float maxDamage = 250.0f;
    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if ((transform.position - prevPos).magnitude >= stepsize)
        {
            GameObject footprint = Instantiate(Resources.Load<GameObject>(prefabPath), transform.position + Vector3.forward * MapManager.MAP_DEPTH / 2.0f, transform.rotation);
            footprint.tag = "Disposable";
            Footprint script = footprint.AddComponent<Footprint>();
            script.SetContactDPS(contactDPS);
            script.SetHostility(GetComponent<Character>().GetHostility());
            script.SetMaxDamage(maxDamage);
            script.SetMaxDuration(maxDuration);
            prevPos = transform.position;
            stepsize = Random.Range(minStepsize, maxStepsize);
        }
    }

    public override string GetDescription()
    {
        return description;
    }

    public override Sprite GetLogo()
    {
        return Resources.Load<Sprite>(logoPath);
    }

    public override string GetName()
    {
        return itemName;
    }

    private class Footprint : MonoBehaviour
    {
        private float alpha = 1.0f;
        private float maxDuration;
        private float maxDamage;
        private float contactDPS;
        private bool hostility;

        public float GetContactDPS() { return contactDPS; }
        public void SetContactDPS(float value) { contactDPS = value; }
        public bool GetHostility() { return hostility; }
        public void SetHostility(bool value) { hostility = value; }
        public float GetMaxDuration() { return maxDuration; }
        public void SetMaxDuration(float value) { maxDuration = value; }
        public float GetMaxDamage() { return maxDamage; }
        public void SetMaxDamage(float value) { maxDamage = value; }
        void Update()
        {
            alpha -= Time.deltaTime / maxDuration;
            if (alpha > 0.0f)
            {
                Color color = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);
            }
            else
                Destroy(gameObject);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.GetHostility() != hostility)
            {
                alpha -= contactDPS * Time.deltaTime / maxDamage;
                new Damage(gameObject, null, damageable, contactDPS * Time.deltaTime).Apply();
            }
        }
    }
}