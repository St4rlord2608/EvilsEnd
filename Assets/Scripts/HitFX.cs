using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem damageEffect;

    private CanBeHit canBeHit;

    private void Awake()
    {
        canBeHit = GetComponent<CanBeHit>();
    }
    void Start()
    {
        canBeHit.OnHit += CanBeHit_OnHit;
    }

    private void CanBeHit_OnHit(object sender, CanBeHit.OnHitEventArgs e)
    {
        ParticleSystem FX = Instantiate(damageEffect, e.hitPosition, Quaternion.identity);
        var direction = (e.hitPosition - Camera.main.transform.position).normalized;
        FX.transform.forward = direction;
        
    }

    void Update()
    {

    }
}
