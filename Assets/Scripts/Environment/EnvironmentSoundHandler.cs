using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSoundHandler : MonoBehaviour
{
    [SerializeField] private EnvironmentSoundScriptableObject soundData;

    public AudioClip GetRandomWalkingFootstep()
    {
        int randomIndex = Random.Range(0, soundData.footstepWalkAudioClip.Length);
        return soundData.footstepWalkAudioClip[randomIndex];
    }

    public AudioClip GetRandomRunningFootstep()
    {
        int randomIndex = Random.Range(0, soundData.footstepRunAudioClip.Length);
        return soundData.footstepRunAudioClip[randomIndex];
    }
}
