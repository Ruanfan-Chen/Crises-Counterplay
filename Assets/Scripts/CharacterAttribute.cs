using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    private float health;
    private float maxHealth;
    private float moveSpeed;
    private float projectileSpeed = 10.0f;
    private float attackInterval = 0.5f;

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float value)
    {
        health = value;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public void SetProjectileSpeed(float value)
    {
        projectileSpeed = value;
    }

    public float GetAttackInterval()
    {
        return attackInterval;
    }

    public void SetAttackInterval(float value)
    {
        attackInterval = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
