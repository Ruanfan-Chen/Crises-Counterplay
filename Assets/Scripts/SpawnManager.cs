using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("A switch to turn enemy spawn on and off for testing and debugging.")]
    [SerializeField] private bool isSpawn = true;
    public GameObject player;
    private float offset = 2.0f;
    private float startDelay = 2.0f;
    private float spawnInterval = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        if (isSpawn)
        {
            InvokeRepeating("SpawnRandomEnemy", startDelay, spawnInterval);
        }
    }

    public GameObject SpawnRandomEnemy()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-120, 120), Random.Range(-120, 120), 0);
        } while (!GetComponent<MapManager>().IsInMap(position, offset) || (position - player.transform.position).magnitude <= offset);
        GameObject enemy = Enemy.Instantiate(position, new Quaternion());
        enemy.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        switch (Random.Range(0, 4))
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
                break;
        }

        switch (Random.Range(0, 3))
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
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<RandomAttackOnDeath>();
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<RingAttackOnDeath>();
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

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }
        public GameObject GetCenter() { return center; }

        public void SetCenter(GameObject value) { center = value; }

        public float GetRange() { return GetComponent<Enemy>().GetRange(); }

        void Update()
        {
            Vector3 relativePos = transform.position - center.transform.position;
            transform.Translate(GetSpeed() * Time.deltaTime * (Quaternion.Euler(0, 0, 2 * Mathf.Atan2(relativePos.magnitude, GetRange()) * Mathf.Rad2Deg) * relativePos.normalized));
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
            Projectile.InstantiateRing(transform.position, 0, GetComponents<IProjectileModifier>(), 6);
        }
    }
}
