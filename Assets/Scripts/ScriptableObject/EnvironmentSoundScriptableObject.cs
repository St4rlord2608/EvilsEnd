using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentSoundScriptableObject")]
public class EnvironmentSoundScriptableObject : ScriptableObject
{
    public AudioClip[] footstepWalkAudioClip;
    public AudioClip[] footstepRunAudioClip;
}
