using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour, IItem
{
    public abstract string GetDescription();
    public abstract Sprite GetLogo();
    public abstract string GetName();
}

public abstract class ActiveItem : MonoBehaviour, IItem
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract bool IsUsable();
    public abstract float GetChargeProgress();
    public abstract string GetDescription();
    public abstract Sprite GetLogo();
    public abstract string GetName();
}

public interface IItem
{
    public abstract string GetName();
    public abstract string GetDescription();
    public abstract Sprite GetLogo();
}

public interface IProjectileModifier
{
    public void Modify(GameObject projectile);
}

public interface IDamageable
{
    bool GetHostility();

    public Coroutine StartCoroutine(IEnumerator routine);

    void ReceiveDamage(Damage damage);
}

public interface IOnDeathEffect
{
    void OnDeath();
}

public interface IWeapon
{
    public float GetDamage();
    public void SetDamage(float value);

    public float GetRange();

    public void SetRange(float value);

    public float GetProjectileSpeed();

    public void SetProjectileSpeed(float value);

    public float GetAngleOfView();

    public void SetAngleOfView(float value);

    public float GetAttackInterval();

    public void SetAttackInterval(float value);
}

public class Damage
{
    private GameObject source;
    private GameObject medium;
    private IDamageable target;
    private float value;
    private IEnumerator coroutine;

    public Damage(GameObject source, GameObject medium, IDamageable target, float value)
    {
        this.source = source;
        this.medium = medium;
        this.target = target;
        this.value = value;
        this.coroutine = null;
    }

    public Damage(GameObject source, GameObject medium, IDamageable target, float value, IEnumerator coroutine)
    {
        this.source = source;
        this.medium = medium;
        this.target = target;
        this.value = value;
        this.coroutine = coroutine;
    }

    public GameObject GetSource() { return source; }

    public void SetSource(GameObject value) { source = value; }

    public GameObject GetMedium() { return medium; }

    public void SetMedium(GameObject value) { medium = value; }

    public IDamageable GetTarget() { return target; }

    public void SetTarget(IDamageable value) { target = value; }

    public float GetValue() { return value; }

    public void SetValue(float value) { this.value = value; }

    public IEnumerator GetCoroutine() { return coroutine; }

    public void SetCoroutine(IEnumerator value) { coroutine = value; }

    public void Apply()
    {
        target.ReceiveDamage(this);
        if (coroutine != null)
            target.StartCoroutine(coroutine);
    }

    public static void Apply(Damage damage) { damage.Apply(); }
}