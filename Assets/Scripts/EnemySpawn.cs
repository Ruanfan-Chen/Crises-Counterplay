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
        List<System.Type> components = new();
        switch (Random.Range(0, 4))
        {
            case 0:
                break;
            case 1:
                components.Add(typeof(AimlesslyMove));
                break;
            case 2:
                components.Add(typeof(DirectlyMoveToward));
                break;
            case 3:
                components.Add(typeof(MoveInCircle));
                break;
        }
        if (!LevelManager.GetEnemieDisarm())
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    break;
                case 1:
                    components.Add(typeof(RandomlyAttack));
                    break;
                case 2:
                    components.Add(typeof(FocusedAttack));
                    break;
            }
        }
        switch (Random.Range(0, 3))
        {
            case 0:
                break;
            case 1:
                components.Add(typeof(RandomAttackOnDeath));
                break;
            case 2:
                components.Add(typeof(RingAttackOnDeath));
                break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                break;
            case 1:
                components.Add(typeof(Waterblight));
                break;
        }
        GameObject enemy = Enemy.Instantiate(position, Quaternion.identity, components.ToArray());
        return enemy;
    }

    public class AimlesslyMove : MonoBehaviour
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

    public class DirectlyMoveToward : MonoBehaviour
    {

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        public Transform GetTarget() { return GameplayManager.getCharacter().transform; }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, GetTarget().position, GetSpeed() * Time.deltaTime);
        }
    }

    public class MoveInCircle : MonoBehaviour
    {
        private float radius = 5.0f;
        private Vector3 currentDir = Vector3.zero;

        private float GetSpeed() { return GetComponent<Enemy>().GetMoveSpeed(); }

        private Transform GetCenter() { return GameplayManager.getCharacter().transform; }

        void Update()
        {
            Vector3 relativePos = transform.position - GetCenter().position;
            currentDir = Vector3.Cross(relativePos, currentDir).z switch
            {
                > 0.0f => Quaternion.Euler(0, 0, 2 * Mathf.Atan2(relativePos.magnitude, radius) * Mathf.Rad2Deg) * relativePos.normalized,
                < 0.0f => Quaternion.Euler(0, 0, -2 * Mathf.Atan2(relativePos.magnitude, radius) * Mathf.Rad2Deg) * relativePos.normalized,
                _ => Random.onUnitSphere
            };
            transform.Translate(GetSpeed() * Time.deltaTime * currentDir);
        }
    }

    public class RandomlyAttack : MonoBehaviour, IAggressive
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

    public class FocusedAttack : MonoBehaviour, IAggressive
    {
        private float minAttackInterval = 1.0f;
        private float maxAttackInterval = 3.0f;
        private float timer = 0.0f;

        public Transform GetTarget() { return GameplayManager.getCharacter().transform; }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                Projectile.Instantiate(transform.position, GetTarget().position, GetComponents<IProjectileModifier>());
                timer = Random.Range(minAttackInterval, maxAttackInterval);
            }
        }
    }

    public class RandomAttackOnDeath : MonoBehaviour, IOnDeathEffect
    {
        void IOnDeathEffect.OnDeath()
        {
            Projectile.Instantiate(transform.position, Random.Range(0.0f, 360.0f), GetComponents<IProjectileModifier>());
        }
    }

    public class RingAttackOnDeath : MonoBehaviour, IOnDeathEffect
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

    public interface IAggressive { }
}
