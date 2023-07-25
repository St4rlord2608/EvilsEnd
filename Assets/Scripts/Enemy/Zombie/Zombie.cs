using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class Zombie : Enemy
{
    private enum MovementState
    {
        isStanding,
        isWalking,
        isRunning
    }

    [SerializeField] private LayerMask playerLayer;
    [Space]
    [SerializeField] private Rig torsoHitRig;
    [SerializeField] private Transform torsoHitTarget;
    [Space]
    [SerializeField] private Rig rightArmHitRig;
    [SerializeField] private Transform rightArmHitTarget;
    [Space]
    [SerializeField] private Rig leftArmHitRig;
    [SerializeField] private Transform leftArmHitTarget;
    [Space]
    [SerializeField] private float maxHealth;
    [Space]
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float rbPushForce = 1;
    [Space]
    [SerializeField] private float headShotDamageMultiplier = 5.0f;
    [SerializeField] private float armShotDamageMultiplier = 0.2f;
    [SerializeField] private float legShotDamageMultiplier = 0.2f;


    private PlayerMovement playerMovement;
    private PlayerShootingHandler playerShootingHandler;
    private Zombie zombie;
    private ZombieAttack zombieAttack;
    private Transform player;
    private float playerMovementNoise;
    private float playerShootingNoise;
    private RichAI aIPath;
    private MovementState movementState;
    private float patrolCooldown;
    private float patrolCurrentCooldown;
    private Vector3 currentTargetPosition;
    private Transform currentTarget;

    private bool torsoGotHit;
    private bool rightArmGotHit;
    private bool leftArmGotHit;
    private bool isChasingPlayer;
    public bool isChasingSomething;
    private bool canAttack;

    public bool isStanding;
    public bool isWalking;
    public bool isRunning;

    protected override void Awake()
    {
        base.Awake();
        aIPath = GetComponent<RichAI>();
        zombieAttack = GetComponent<ZombieAttack>();
    }

    void Start()
    {
        torsoHitRig.weight = 0.0f;
        rightArmHitRig.weight = 0.0f;
        leftArmHitRig.weight = 0.0f;
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleDeath();
        torsoGotHit = HandleHitReaction(torsoGotHit, torsoHitRig);
        rightArmGotHit = HandleHitReaction(rightArmGotHit, rightArmHitRig);
        leftArmGotHit = HandleHitReaction(leftArmGotHit, leftArmHitRig);
        HandleMovementState();
        HandlePatrol();
        TryToAttack();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0f;
            forceDirection.Normalize();

            rb.AddForceAtPosition(forceDirection * rbPushForce, transform.position, ForceMode.Impulse);
        }
    }

    private bool HandleHitReaction(bool bodyPartGotHit, Rig bodyPartRig)
    {
        if (bodyPartGotHit)
        {
            bodyPartRig.weight = Mathf.Lerp(bodyPartRig.weight, 1.0f, 10f * Time.deltaTime);
            if (bodyPartRig.weight >= 0.9f)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        if (bodyPartRig.weight > 0.0f && bodyPartGotHit == false)
        {
            bodyPartRig.weight = Mathf.Lerp(bodyPartRig.weight, 0.0f, 10f * Time.deltaTime);
        }
        return false;
    }

    private void TryToAttack()
    {
        if(player != null)
        {
            if(!canAttack)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 1.5)
                {
                    isChasingPlayer = true;
                    canAttack = true;
                    var lookPos = player.transform.position - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Infinity);
                }
                else
                {
                    canAttack = false;
                }
            }
        }
        else
        {
            canAttack = false;
        }
    }

    private void HandleMovementState()
    {
        if (aIPath.velocity.magnitude <= 0.01)
        {
            isChasingPlayer = false;
            isChasingSomething = false;
            movementState = MovementState.isStanding;
        }
        else if(!isChasingPlayer && !zombieAttack.GetIsAttacking() && !isChasingSomething)
        {
            movementState = MovementState.isWalking;
            aIPath.maxSpeed = walkingSpeed;
        }
        else if (isChasingPlayer || isChasingSomething)
        {
            movementState = MovementState.isRunning;
            aIPath.maxSpeed = runningSpeed;
        }

        switch (movementState)
        {
            case MovementState.isStanding:
                isStanding = true;
                isWalking = false;
                isRunning = false;
                break;
            case MovementState.isWalking:
                isStanding = false;
                isWalking = true;
                isRunning = false;
                break;
            case MovementState.isRunning:
                isStanding = false;
                isWalking = false;
                isRunning = true;
                break;
        }
    }

    private void HandlePatrol()
    {
        if(!isChasingPlayer && aIPath.velocity.magnitude <= 0.1 && !canAttack)
        {
             if (patrolCurrentCooldown >= patrolCooldown)
             {
                 float randomXPosition = Random.Range(-10.0f, 10.0f);
                 float randomZPosition = Random.Range(-10.0f, 10.0f);

                 aIPath.destination = new Vector3(transform.position.x + randomXPosition,
                     transform.position.y, transform.position.z + randomZPosition);
                patrolCurrentCooldown = 0;
                patrolCooldown = Random.Range(2.0f, 10.0f);
             }
             else
             {
                 patrolCurrentCooldown += Time.deltaTime;
             }
        }
        else
        {
            patrolCurrentCooldown = 0;
        }
    }

    public override void HearsPlayer(Transform player)
    {
        if(MusicManager.Instance != null)
        {
            MusicManager.Instance.CombatTrigger();
        }
        isChasingSomething = false;
        if (player.TryGetComponent<PlayerMovement>(out playerMovement) && player.TryGetComponent<PlayerShootingHandler>(out playerShootingHandler))
        {
            this.player = player;
            if (Vector3.Distance(transform.position, player.transform.position) < 1.5)
            {
                canAttack = true;
                var lookPos = player.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Mathf.Infinity);
            }
            else
            {
                canAttack = false;
            }
            playerMovementNoise = playerMovement.GetCurrentMovementNoise();
            isChasingPlayer = true;
            aIPath.destination = player.transform.position;
            currentTargetPosition = player.transform.position;
            currentTarget = player.transform;

            playerMovement = null;
            playerShootingHandler = null;
        }
    }

    public override void HearsSomething(Transform noiseTransform)
    {
        if(!isChasingPlayer)
        {
            isChasingSomething = true;
            aIPath.destination = noiseTransform.position;
        }
        else
        {
            isChasingSomething = false;
        }
    }

    public override void TakeDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        base.TakeDamage(hitPosition, damage, damageSource);
        currentHealth -= damage;
        if (damageSource.TryGetComponent<PlayerShootingHandler>(out PlayerShootingHandler playerShootingHandler))
        {
            if(currentHealth <= 0)
            {
                HitIndicationUI.Instance.ShowKill();
            }
            else
            {
                HitIndicationUI.Instance.ShowHit();
            }
        }
    }

    public override void RightArmDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        rightArmHitTarget.position = hitPosition;
        rightArmGotHit = true;
        TakeDamage(hitPosition, damage * armShotDamageMultiplier, damageSource);
    }

    public override void LeftArmDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        leftArmHitTarget.position = hitPosition;
        leftArmGotHit = true;
        TakeDamage(hitPosition, damage * armShotDamageMultiplier, damageSource);
    }

    public override void TorsoDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        torsoHitTarget.position = hitPosition;
        torsoGotHit = true;
        TakeDamage(hitPosition, damage, damageSource);
    }

    public override void RightLegDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage * legShotDamageMultiplier, damageSource);
    }

    public override void LeftLegDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage * legShotDamageMultiplier, damageSource);
    }

    public override void HeadDamage(Vector3 hitPosition, float damage, Transform damageSource)
    {
        TakeDamage(hitPosition, damage * headShotDamageMultiplier, damageSource);
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }

    public bool GetIsChasingSomething()
    {
        return isChasingPlayer;
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
}
