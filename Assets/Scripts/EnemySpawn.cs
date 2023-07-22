using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private float offset = 2.0f;
    [SerializeField] private float startDelay = 3.0f;
    [SerializeField] private float spawnInterval = 0.3f;

    private float timer;
    void Start()
    {

        timer = startDelay;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f && LevelManager.GetSpawnEnemy() && !BlockEnemySpawn.ExistInstance())
        {
            SpawnRandomEnemy();
            timer = spawnInterval;
        }
    }

    public GameObject SpawnRandomEnemy()
    {
        Vector3 position;
        do
        {
            position = MapManager.GetRandomPointInMap();
        } while ((position - GameplayManager.getCharacter().transform.position).magnitude <= offset);
        GameObject enemy = Enemy.Instantiate(position, Quaternion.identity);
        enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        switch (Random.Range(0, 4))
        {
            case 0:
                break;
            case 1:
                enemy.AddComponent<AimlesslyMove>();
                break;
            case 2:
                enemy.AddComponent<DirectlyMoveToward>();
                enemy.GetComponent<DirectlyMoveToward>().SetTarget(GameplayManager.getCharacter().transform);
                break;
            case 3:
                enemy.AddComponent<MoveInCircle>();
                enemy.GetComponent<MoveInCircle>().SetCenter(GameplayManager.getCharacter().transform);
                enemy.GetComponent<MoveInCircle>().SetRadius(5.0f);
                break;
        }
        if (!LevelManager.GetEnemieDisarm())
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    break;
                case 1:
                    enemy.AddComponent<RandomlyAttack>();
                    break;
                case 2:
                    enemy.AddComponent<FocusedAttack>();
                    enemy.GetComponent<FocusedAttack>().SetTarget(GameplayManager.getCharacter().transform);
                    break;
            }
        }
        switch (Random.Range(0, 3))
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
        switch (Random.Range(0, 2))
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
        private Transform target;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        public void SetTarget(Transform value) { target = value; }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, GetSpeed() * Time.deltaTime);
        }
    }

    private class MoveInCircle : MonoBehaviour
    {
        private Transform center;
        private float radius;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        public void SetRadius(float value) { radius = value; }

        public void SetCenter(Transform value) { center = value; }

        void Update()
        {
            Vector3 relativePos = transform.position - center.position;
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
        private Transform target;
        private float minAttackInterval = 1.0f;
        private float maxAttackInterval = 3.0f;
        private float timer = 0.0f;

        public void SetTarget(Transform value) { target = value; }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                Projectile.Instantiate(transform.position, target.position, GetComponents<IProjectileModifier>());
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

    public class Patrol : MonoBehaviour
    {
        private Vector3 patrolPointA = new(0.0f, 15.0f);
        private Vector3 patrolPointB = new(0.0f, -15.0f);
        private bool status = false;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        private Vector3 GetTargetPos() { return status ? patrolPointA : patrolPointB; }

        public void SetPatrolPointA(Vector3 value)
        {
            patrolPointA = value;
        }

        public void SetPatrolPointB(Vector3 value)
        {
            patrolPointB = value;
        }

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, GetTargetPos(), GetSpeed() * Time.deltaTime);
            if (transform.position == GetTargetPos())
                status = !status;
        }
    }
}
