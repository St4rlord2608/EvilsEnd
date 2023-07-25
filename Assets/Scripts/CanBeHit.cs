using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class CanBeHit : MonoBehaviour
{
    public event EventHandler<OnHitEventArgs> OnHit;

    protected Vector3 lastHitPosition;
    protected float lastHitDamage;

    public class OnHitEventArgs : EventArgs
    {
        public Transform damageSource;
        public Vector3 hitPosition;
    }
    public virtual void TakeDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        lastHitDamage = damage;
        lastHitPosition = hitPosition;
        OnHit?.Invoke(this, new OnHitEventArgs()
        {
            damageSource = damageSource,
            hitPosition = hitPosition,
        });
    }

    public virtual void RightArmDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage, damageSource);
    }

    public virtual void LeftArmDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage, damageSource);
    }

    public virtual void TorsoDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage, damageSource);
    }

    public virtual void RightLegDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage, damageSource);
    }

    public virtual void LeftLegDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage, damageSource);
    }

    public virtual void HeadDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage, damageSource);
    }
}
