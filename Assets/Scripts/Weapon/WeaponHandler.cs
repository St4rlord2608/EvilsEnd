using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform currentEquippedMagazine;
    [SerializeField] private WeaponScriptableObject weaponData;

    public Transform GetCurrentEquippedMagazine()
    {
        return currentEquippedMagazine;
    }

    public string GetWeaponType()
    {
        return weaponData.weaponType;
    }
    public float GetDefaultRecoilPower()
    {
        return weaponData.defaultRecoilPower;
    }

    public float GetSprintingRecoilPower()
    {
        return weaponData.sprintingRecoilPower;
    }

    public float GetCrouchingRecoilPower()
    {
        return weaponData.crouchingRecoilPower;
    }

    public float GetWeaponDamage()
    {
        return weaponData.damage;
    }

    public AudioClip GetMagazineRemovedAudioClip()
    {
        return weaponData.magazineRemoveAudioClip;
    }

    public AudioClip GetMagazineAttachedAudioClip()
    {
        return weaponData.magazineAttachedAudioClip;
    }

    public AudioClip GetReloadLeverPullAudioClip()
    {
        return weaponData.reloadLeverPullAudioClip;
    }
}
