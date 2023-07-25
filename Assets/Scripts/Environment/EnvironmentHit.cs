using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHit : CanBeHit
{
    public override void TakeDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        base.TakeDamage(hitPosition, damage, damageSource);
    }
}
