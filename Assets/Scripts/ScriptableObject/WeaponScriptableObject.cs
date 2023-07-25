using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponScriptableObject")]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponType;
    public float defaultRecoilPower;
    public float sprintingRecoilPower;
    public float crouchingRecoilPower;
    public float damage;
    public AudioClip magazineRemoveAudioClip;
    public AudioClip magazineAttachedAudioClip;
    public AudioClip reloadLeverPullAudioClip;
}
