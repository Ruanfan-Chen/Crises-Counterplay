using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private float offset = 2.0f;
    [SerializeField] private float startDelay = 3.0f;
    [SerializeField] private float spawnInterval = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay, spawnInterval);
    }

    public GameObject SpawnRandomEnemy()
    {
        Vector3 position;
        Bounds bounds = MapManager.GetMapBounds(offset);
        do
        {
            position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
        } while ((position - player.transform.position).magnitude <= offset);
        GameObject enemy = Enemy.Instantiate(position, Quaternion.identity);
        enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        //int move = Random.Range(0, 4);
        int move = 2;
        switch (move)
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<AimlesslyMove>();
                break;
            case 2:
                enemy.AddComponent<DirectlyMoveToward>();
                enemy.GetComponent<DirectlyMoveToward>().SetTarget(player);
                break;
            case 3:
                enemy.AddComponent<MoveInCircle>();
                enemy.GetComponent<MoveInCircle>().SetCenter(player);
                enemy.GetComponent<MoveInCircle>().SetRadius(5.0f);
                break;
        }
        int attack = 0;
        switch (attack)
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<RandomlyAttack>();
                break;
            case 2:
                enemy.AddComponent<FocusedAttack>();
                enemy.GetComponent<FocusedAttack>().SetTarget(player);
                break;
        }
        int onDeath = 0;
        switch (onDeath)
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<RandomAttackOnDeath>();
                enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
                break;
            case 2:
                enemy.AddComponent<RingAttackOnDeath>();
                enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
        }
        int water = 0;
        switch (water)
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<Waterblight>();
                break;
        }
        return enemy;
    }


    private class AimlesslyMove : MonoBehaviour
    {
        private float timer;
        private Vector3 direction;
        private float minHaltTime = 0.5f;
        private float maxHaltTime = 1.5f;
        private float minMoveDistance = 2.5f;
        private float maxMoveDistance = 5.0f;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        // Start is called before the first frame update
        void Start()
        {
            Halt(0);
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            transform.Translate(GetSpeed() * Time.deltaTime * direction);
            if (timer <= 0)
            {
                if (direction.magnitude == 0)
                {
                    float theta = Random.Range(0.0f, 2 * Mathf.PI);
                    Move(Random.Range(minMoveDistance, maxMoveDistance) * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0));
                }
                else
                    Halt(Random.Range(minHaltTime, maxHaltTime));
            }
        }
        void Halt(float seconds)
        {
            timer = seconds;
            direction = Vector3.zero;
        }

        void Move(Vector3 displacement)
        {
            timer = displacement.magnitude / GetSpeed();
            direction = displacement.normalized;
        }
    }

    private class DirectlyMoveToward : MonoBehaviour
    {
        private GameObject target;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        public GameObject GetTarget() { return target; }

        public void SetTarget(GameObject value) { target = value; }

        // Update is called once per frame
        void Update()
        {
            Vector3 displacement = target.transform.position - transform.position;
            float speed = GetSpeed();
            if (speed * Time.deltaTime < displacement.magnitude)
                transform.Translate(speed * Time.deltaTime * displacement.normalized);
            else
                transform.Translate(displacement);
        }
    }

    private class MoveInCircle : MonoBehaviour
    {
        private GameObject center;
        private float radius;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }
        public GameObject GetCenter() { return center; }

        public float GetRadius()        {            return radius;        }

        public void SetRadius(float value)        {            radius = value;        }

        public void SetCenter(GameObject value) { center = value; }

        void Update()
        {
            Vector3 relativePos = transform.position - center.transform.position;
            transform.Translate(GetSpeed() * Time.deltaTime * (Quaternion.Euler(0, 0, 2 * Mathf.Atan2(relativePos.magnitude, radius) * Mathf.Rad2Deg) * relativePos.normalized));
        }
    }

    private class RandomlyAttack : MonoBehaviour
    {
        private float minAttackInterval = 1.0f;
        private float maxAttackInterval = 3.0f;
        private float timer = 0.0f;

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                Projectile.Instantiate(transform.position, Random.Range(0.0f, 360.0f), GetComponents<IProjectileModifier>());
                timer = Random.Range(minAttackInterval, maxAttackInterval);
            }
        }
    }

    private class FocusedAttack : MonoBehaviour
    {
        private GameObject target;
        private float minAttackInterval = 1.0f;
        private float maxAttackInterval = 3.0f;
        private float timer = 0.0f;

        public GameObject GetTarget() { return target; }

        public void SetTarget(GameObject value) { target = value; }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                Projectile.Instantiate(transform.position, target.transform.position, GetComponents<IProjectileModifier>());
                timer = Random.Range(minAttackInterval, maxAttackInterval);
            }
        }
    }

    private class RandomAttackOnDeath : MonoBehaviour, IOnDeathEffect
    {
        void IOnDeathEffect.OnDeath()
        {
            Projectile.Instantiate(transform.position, Random.Range(0.0f, 360.0f), GetComponents<IProjectileModifier>());
        }
    }

    private class RingAttackOnDeath : MonoBehaviour, IOnDeathEffect
    {
        void IOnDeathEffect.OnDeath()
        {
            Projectile.InstantiateRing(transform.position, Random.Range(0.0f, 360.0f), GetComponents<IProjectileModifier>(), 6);
        }
    }
}
