using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private float reloadingLayerWeightReductionSpeed = 2.0f;
    [SerializeField] private float crouchLayerWeightSpeed = 2.0f;

    private const string IS_HOLSTERED_WALKING_FORWARD = "IsHolsteredWalkingForward";
    private const string IS_AIMING = "IsAiming";
    private const string IS_WALKING_AIMED = "IsWalkingAimed";
    private const string IS_HOLSTERED_RUNNING_FORWARD = "IsHolsteredRunningForward";
    private const string IS_RUNNING_AIMED = "IsRunningAimed";
    private const string MOVEMENT_Y = "MovementY";
    private const string MOVEMENT_X = "MovementX";
    private const string RELOAD = "Reload";
    private const string RELOAD_BULLET_IN_CHAMBER = "ReloadWithBulletInChamber";
    private const string RELOADING_LAYER = "Reloading";
    private const string CROUCH_LAYER = "Crouching";

    private PlayerMovement playerMovement;
    private PlayerAim playerAim;
    private Animator animator;
    private PlayerReloading playerReloading;
    private AnimationEventHandler animationEventHandler;
    private int ReloadingLayerIndex;
    private int CrouchingLayerIndex;

    private bool isReloading = false;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAim = GetComponent<PlayerAim>();
        animator = GetComponent<Animator>();
        playerReloading = GetComponent<PlayerReloading>();
        animationEventHandler = GetComponent<AnimationEventHandler>();
    }

    private void Start()
    {
        playerReloading.OnReload += PlayerReloading_OnReload;
        animationEventHandler.OnFinish += AnimationEventHandler_OnFinish;
        ReloadingLayerIndex = animator.GetLayerIndex(RELOADING_LAYER);
        CrouchingLayerIndex = animator.GetLayerIndex(CROUCH_LAYER);
        animator.SetLayerWeight(ReloadingLayerIndex, 0.0f);
        animator.SetLayerWeight(CrouchingLayerIndex, 0.0f);
    }

    private void AnimationEventHandler_OnFinish(object sender, System.EventArgs e)
    {
        isReloading = false;
    }

    private void PlayerReloading_OnReload(object sender, PlayerReloading.ReloadEventArgs e)
    {
        animator.SetLayerWeight(ReloadingLayerIndex, 1.0f);
        if (!e.bulletInChamber)
        {
            animator.SetTrigger(RELOAD);
        }
        else
        {
            animator.SetTrigger(RELOAD_BULLET_IN_CHAMBER);
        }
        isReloading = true;
    }

    private void Update()
    {
        animator.SetBool(IS_HOLSTERED_WALKING_FORWARD, playerMovement.isHolsteredWalkingForward);
        animator.SetBool(IS_AIMING, playerMovement.isAiming);
        animator.SetBool(IS_WALKING_AIMED, playerAim.isWalingAimed);
        animator.SetBool(IS_HOLSTERED_RUNNING_FORWARD, playerMovement.isHolsteredRunningForward);
        animator.SetFloat(MOVEMENT_X, playerAim.GetCurrentMovementVector().x);
        animator.SetFloat(MOVEMENT_Y, playerAim.GetCurrentMovementVector().y);
        animator.SetBool(IS_RUNNING_AIMED, playerAim.isRunningAimed);

        if (!isReloading && animator.GetLayerWeight(ReloadingLayerIndex) > 0)
        {
            animator.SetLayerWeight(ReloadingLayerIndex, Mathf.Lerp(animator.GetLayerWeight(ReloadingLayerIndex), 0.0f, reloadingLayerWeightReductionSpeed * Time.deltaTime));
        }
        if(playerMovement.isCrouching && animator.GetLayerWeight(CrouchingLayerIndex) < 1.0f)
        {
            animator.SetLayerWeight(CrouchingLayerIndex, Mathf.Lerp(animator.GetLayerWeight(CrouchingLayerIndex), 1.0f, crouchLayerWeightSpeed * Time.deltaTime));
        }else if(!playerMovement.isCrouching && animator.GetLayerWeight(CrouchingLayerIndex) > 0.0f)
        {
            animator.SetLayerWeight(CrouchingLayerIndex, Mathf.Lerp(animator.GetLayerWeight(CrouchingLayerIndex), 0.0f, crouchLayerWeightSpeed * Time.deltaTime));
        }
    }

    private void OnDestroy()
    {
        playerReloading.OnReload -= PlayerReloading_OnReload;
    }

}
