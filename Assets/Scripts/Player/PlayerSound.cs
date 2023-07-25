using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    

    [SerializeField] private AudioClip shot;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform RightFootTransform;
    [SerializeField] private Transform LeftFootTransform;
    [Space]
    [SerializeField] private float leftFootSoundMaxCooldown = 0.1f;
    [SerializeField] private float rightFootSoundMaxCooldown = 0.1f;
    [Space]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckLength = +-2f;
    [Space]
    [SerializeField] private float sneakMusicTriggerRadius = 20f;


    private PlayerShootingHandler playerShootingHandler;
    private PlayerMovement playerMovement;
    private AnimationEventHandler playerAnimationEventHandler;
    private PlayerWeaponHandler playerWeaponHandler;

    private float leftFootSoundCurrentCooldown;
    private float rightFootSoundCurrentCooldown;
    private void Awake()
    {
        leftFootSoundCurrentCooldown = leftFootSoundMaxCooldown;
        rightFootSoundCurrentCooldown = rightFootSoundMaxCooldown;

        playerShootingHandler = GetComponent<PlayerShootingHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimationEventHandler = GetComponent<AnimationEventHandler>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();

        playerShootingHandler.OnShooting += PlayerShootingHandler_OnShooting;
        playerAnimationEventHandler.OnLeftFootStep += PlayerAnimationEventHandler_OnLeftFootStep;
        playerAnimationEventHandler.OnRightFootStep += PlayerAnimationEventHandler_OnRightFootStep;
        playerAnimationEventHandler.OnLeftFootStepRun += PlayerAnimationEventHandler_OnLeftFootStepRun;
        playerAnimationEventHandler.OnRightFootStepRun += PlayerAnimationEventHandler_OnRightFootStepRun;
        playerAnimationEventHandler.OnMagazineRemoved += PlayerAnimationEventHandler_OnMagazineRemoved;
        playerAnimationEventHandler.OnMagazineAttached += PlayerAnimationEventHandler_OnMagazineAttached;
        playerAnimationEventHandler.OnPullReloadLever += PlayerAnimationEventHandler_OnPullReloadLever;
        playerAnimationEventHandler.OnAttachMagazine += PlayerAnimationEventHandler_OnAttachMagazine;
    }

    private void PlayerAnimationEventHandler_OnAttachMagazine(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(playerWeaponHandler.GetMagazineAttachedAudioClip(), playerWeaponHandler.GetShootingPoint().position, 1f);
    }

    private void PlayerAnimationEventHandler_OnPullReloadLever(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(playerWeaponHandler.GetReloadLeverPullAudioClip(), playerWeaponHandler.GetShootingPoint().position, 1f);
    }

    private void PlayerAnimationEventHandler_OnMagazineAttached(object sender, System.EventArgs e)
    {
        
    }

    private void PlayerAnimationEventHandler_OnMagazineRemoved(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(playerWeaponHandler.GetMagazineRemovedAudioClip(), playerWeaponHandler.GetShootingPoint().position, 1f);
    }

    private void PlayerAnimationEventHandler_OnRightFootStepRun(object sender, System.EventArgs e)
    {
        if (rightFootSoundCurrentCooldown >= rightFootSoundMaxCooldown)
        {
            if (Physics.Raycast(RightFootTransform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, groundCheckLength, groundLayers))
            {
                if(hitInfo.transform.TryGetComponent<EnvironmentSoundHandler>(out EnvironmentSoundHandler environmentSoundHandler))
                {
                    SoundManager.Instance.PlaySound(environmentSoundHandler.GetRandomRunningFootstep(), RightFootTransform.position, 1f);
                    rightFootSoundCurrentCooldown = 0;
                }
                
            }
        }
    }

    private void PlayerAnimationEventHandler_OnLeftFootStepRun(object sender, System.EventArgs e)
    {
        if (leftFootSoundCurrentCooldown >= leftFootSoundMaxCooldown)
        {
            if (Physics.Raycast(LeftFootTransform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, groundCheckLength, groundLayers))
            {
                if (hitInfo.transform.TryGetComponent<EnvironmentSoundHandler>(out EnvironmentSoundHandler environmentSoundHandler))
                {
                    SoundManager.Instance.PlaySound(environmentSoundHandler.GetRandomWalkingFootstep(), LeftFootTransform.position, 1f);
                    leftFootSoundCurrentCooldown = 0;
                }
            }

        }
    }

    private void PlayerShootingHandler_OnShooting(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(shot, shootingPoint.position, 0.5f);
    }

    private void PlayerAnimationEventHandler_OnLeftFootStep(object sender, System.EventArgs e)
    {
        if(leftFootSoundCurrentCooldown >= leftFootSoundMaxCooldown)
        {
            if(Physics.Raycast(LeftFootTransform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, groundCheckLength, groundLayers))
            {
                if (hitInfo.transform.TryGetComponent<EnvironmentSoundHandler>(out EnvironmentSoundHandler environmentSoundHandler))
                {
                    SoundManager.Instance.PlaySound(environmentSoundHandler.GetRandomWalkingFootstep(), LeftFootTransform.position, 1f);
                    leftFootSoundCurrentCooldown = 0;
                }
            }
            
        }
        
    }

    private void PlayerAnimationEventHandler_OnRightFootStep(object sender, System.EventArgs e)
    {
        if (rightFootSoundCurrentCooldown >= rightFootSoundMaxCooldown)
        {
            if (Physics.Raycast(RightFootTransform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, groundCheckLength, groundLayers))
            {
                if (hitInfo.transform.TryGetComponent<EnvironmentSoundHandler>(out EnvironmentSoundHandler environmentSoundHandler))
                {
                    SoundManager.Instance.PlaySound(environmentSoundHandler.GetRandomWalkingFootstep(), RightFootTransform.position, 1f);
                    rightFootSoundCurrentCooldown = 0;
                }

            }
        }
    }

    private void Update()
    {
        if(MusicManager.Instance != null)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, sneakMusicTriggerRadius);
            foreach (Collider collider in nearbyColliders)
            {
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    MusicManager.Instance.SneakTrigger();
                }
            }
        }
        
        if (rightFootSoundCurrentCooldown < rightFootSoundMaxCooldown)
        {
            rightFootSoundCurrentCooldown += Time.deltaTime;
        }
        if(leftFootSoundCurrentCooldown < leftFootSoundMaxCooldown)
        {
            leftFootSoundCurrentCooldown += Time.deltaTime;
        }
    }

    private Vector3 GetSpherePosition()
    {
        return new Vector3(transform.position.x, transform.position.y - playerMovement.GetGroundOffset(),
                transform.position.z);
    }
}
