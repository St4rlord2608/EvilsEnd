using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlaySoundOnPositionQuestLock : QuestLock
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float volumeMultiplier = 1f;
    [SerializeField] private float noise = 0f;
    [SerializeField] private int playSoundAmount = 0;

    private bool questCompleted = false;
    private float audioClipLength;
    private float audioClipCurrentLength = 0;
    private int playedSoundCount = 0;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
        audioClipLength = audioClip.length;
        SoundManager.Instance.OnSoundEffectVolumeChange += SoundManager_OnSoundEffectVolumeChange;
    }

    private void SoundManager_OnSoundEffectVolumeChange(object sender, System.EventArgs e)
    {
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
    }

    private void Update()
    {
        if (!questCompleted)
        {
            return;
        }
        if(playedSoundCount != 0)
        {
            audioClipCurrentLength += Time.deltaTime;
            if (audioClipCurrentLength >= audioClipLength)
            {
                if (playedSoundCount >= playSoundAmount)
                {
                    return;
                }
                playedSoundCount++;
                audioClipCurrentLength = 0f;
            }
        }
        if(audioSource.isPlaying)
        {
            Collider[] inNoiseRangeColliders = Physics.OverlapSphere(transform.position, noise);
            foreach (Collider collider in inNoiseRangeColliders)
            {

                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.HearsSomething(transform);
                }
            }
        }
        
    }
    public override void QuestCompleted()
    {
        if (!questCompleted)
        {
            questCompleted = true;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
        
    }
}
