using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableCanBeHit : CanBeHit
{
    [SerializeField] private float rbPushForce = 2f;

    public override void TakeDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        base.TakeDamage(hitPosition, damage, damageSource);
        if(transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 forceDirection = transform.position - hitPosition;
            forceDirection.y = 0f;
            forceDirection.Normalize();

            rb.AddForceAtPosition(forceDirection * rbPushForce, transform.position, ForceMode.Impulse);
        }
    }
}
