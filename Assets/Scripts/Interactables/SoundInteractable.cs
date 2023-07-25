using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInteractable : Interactable
{
    [SerializeField] private float volumeMultiplier = 1.0f; 
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
        SoundManager.Instance.OnSoundEffectVolumeChange += SoundManager_OnSoundEffectVolumeChange;
    }

    private void SoundManager_OnSoundEffectVolumeChange(object sender, System.EventArgs e)
    {
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
    }

    public override void Interact(Transform player)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
}
