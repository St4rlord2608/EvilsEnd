using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float punchDamage = 10f;

    private Zombie zombie;
    private AnimationEventHandler eventHandler;

    private bool canAttack;
    private bool madeHit;

    public bool isAttacking;

    private void Awake()
    {
        zombie = GetComponent<Zombie>();
        eventHandler = GetComponent<AnimationEventHandler>();
    }

    private void OnEnable()
    {
        eventHandler.OnFinish += EventHandler_OnFinish;
    }

    private void OnDisable()
    {
        eventHandler.OnFinish -= EventHandler_OnFinish;
    }

    private void EventHandler_OnFinish(object sender, System.EventArgs e)
    {
        isAttacking = false;
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        canAttack = zombie.GetCanAttack();
        if(canAttack && !isAttacking)
        {
            isAttacking = true;
            madeHit = false;

            
        }else if (isAttacking)
        {
            Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
            foreach (Collider player in hitPlayers)
            {
                if (!madeHit)
                {
                    madeHit = true;
                    if(player.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                    {
                        playerHealth.TakeDamage(punchDamage);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }
}
