using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_ATTACKING = "IsAttacking";

    private Animator animator;
    private Zombie zombie;
    private ZombieAttack zombieAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        zombie= GetComponent<Zombie>();
        zombieAttack = GetComponent<ZombieAttack>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, zombie.isWalking);
        animator.SetBool(IS_RUNNING, zombie.isRunning);
        animator.SetBool(IS_ATTACKING, zombieAttack.GetIsAttacking());
    }
}
