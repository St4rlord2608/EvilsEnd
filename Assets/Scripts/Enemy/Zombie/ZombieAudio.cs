using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] moanAudioClips;
    [SerializeField] private float moanMinTimeout;
    [SerializeField] private float moanMaxTimeout;
    [SerializeField] private float volumeMultiplier = 0.2f;
    [SerializeField] private Transform RightFootTransform;
    [SerializeField] private Transform LeftFootTransform;
    [Space]
    [SerializeField] private float leftFootSoundMaxCooldown = 0.1f;
    [SerializeField] private float rightFootSoundMaxCooldown = 0.1f;
    [Space]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckLength = 2f;


    private float leftFootSoundCurrentCooldown;
    private float rightFootSoundCurrentCooldown;
    private float moanCurrentTimeout = 0;
    private float moanTimeout = 0;
    private float currentClipRestLength = 0;


    private AudioSource audioSource;
    private AnimationEventHandler animationEventHandler;

    
    private bool isTriggered = false;

    private void Awake()
    {
        moanCurrentTimeout = 0f;
        moanTimeout = Random.Range(moanMinTimeout, moanMaxTimeout);
        audioSource = GetComponent<AudioSource>();

        leftFootSoundCurrentCooldown = leftFootSoundMaxCooldown;
        rightFootSoundCurrentCooldown = rightFootSoundMaxCooldown;

        animationEventHandler = GetComponent<AnimationEventHandler>();

        animationEventHandler.OnLeftFootStep += AnimationEventHandler_OnLeftFootStep;
        animationEventHandler.OnRightFootStep += AnimationEventHandler_OnRightFootStep;
        animationEventHandler.OnLeftFootStepRun += AnimationEventHandler_OnLeftFootStepRun;
        animationEventHandler.OnRightFootStepRun += AnimationEventHandler_OnRightFootStepRun;

    }

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
            SoundManager.Instance.OnSoundEffectVolumeChange += SoundManager_OnSoundEffectVolumeChange;
        }
    }

    private void SoundManager_OnSoundEffectVolumeChange(object sender, System.EventArgs e)
    {
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
    }

    private void Update()
    {
        if (!isTriggered)
        {
            if(currentClipRestLength >= 0)
            {
                currentClipRestLength -= Time.deltaTime;
            }
            if(currentClipRestLength <= 0)
            {
                moanCurrentTimeout += Time.deltaTime;
                if (moanCurrentTimeout > moanTimeout)
                {
                    PlayRandomMoan();
                    moanCurrentTimeout = 0f;
                    moanTimeout = Random.Range(moanMinTimeout, moanMaxTimeout);
                }
            }
            
            
        }
        if (rightFootSoundCurrentCooldown < rightFootSoundMaxCooldown)
        {
            rightFootSoundCurrentCooldown += Time.deltaTime;
        }
        if (leftFootSoundCurrentCooldown < leftFootSoundMaxCooldown)
        {
            leftFootSoundCurrentCooldown += Time.deltaTime;
        }
    }

    private void PlayRandomMoan()
    {
        int randomMoanIndex = Random.Range(0, moanAudioClips.Length);
        AudioClip randomMoanClip = moanAudioClips[randomMoanIndex];
        currentClipRestLength = randomMoanClip.length;
        audioSource.clip = randomMoanClip;
        audioSource.Play();
    }

    public void SetIsTriggered(bool isTriggered)
    {
        this.isTriggered = isTriggered;   
    }

    private void AnimationEventHandler_OnRightFootStepRun(object sender, System.EventArgs e)
    {
        if (rightFootSoundCurrentCooldown >= rightFootSoundMaxCooldown)
        {
            if (Physics.Raycast(RightFootTransform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, groundCheckLength, groundLayers))
            {
                if (hitInfo.transform.TryGetComponent<EnvironmentSoundHandler>(out EnvironmentSoundHandler environmentSoundHandler))
                {
                    SoundManager.Instance.PlaySound(environmentSoundHandler.GetRandomRunningFootstep(), RightFootTransform.position, 1f);
                    rightFootSoundCurrentCooldown = 0;
                }

            }
        }
    }

    private void AnimationEventHandler_OnLeftFootStepRun(object sender, System.EventArgs e)
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

    private void AnimationEventHandler_OnLeftFootStep(object sender, System.EventArgs e)
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

    private void AnimationEventHandler_OnRightFootStep(object sender, System.EventArgs e)
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
}
