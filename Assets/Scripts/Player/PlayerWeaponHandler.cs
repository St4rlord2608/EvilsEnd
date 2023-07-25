using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform currentWeaponSecondHandPoint;
    [SerializeField] private int currentWeaponMagazineCapacity = 30;
    [SerializeField] private WeaponHandler weaponHandler;
    
    private int ammunitionInCurrentMagazine;
    private Transform currentWeaponMagazineTransform;

    private void Awake()
    {
    }

    private void Update()
    {
        
    }

    public float GetDefaultRecoilPower()
    {
        return weaponHandler.GetDefaultRecoilPower();
    }

    public float GetSprintingRecoilPower()
    {
        return weaponHandler.GetSprintingRecoilPower();
    }

    public float GetCrouchingRecoilPower()
    {
        return weaponHandler.GetCrouchingRecoilPower();
    }

    public Transform GetGunPivot()
    {
        return gunPivot;
    }

    public Transform GetShootingPoint()
    {
        return shootingPoint;
    }

    public Transform GetCurrentWeaponSecondHandPoint()
    {
        return currentWeaponSecondHandPoint;
    }

    public float GetCurrentWeaponDamage()
    {
        return weaponHandler.GetWeaponDamage();
    }

    public int GetWeaponMagazineCapacity()
    {
        return currentWeaponMagazineCapacity;
    }

    public int GetAmmunitionInCurrentMagazine()
    {
        return ammunitionInCurrentMagazine;
    }

    public void SetAmmunitionInCurrentMagazine(int newAmmunitionInCurrentMagazine)
    {
        ammunitionInCurrentMagazine = newAmmunitionInCurrentMagazine;
        if(AmmunitionUI.Instance != null)
        {
            AmmunitionUI.Instance.SetAmmunitionInCurrentMagazine(ammunitionInCurrentMagazine);
        }
    }

    public string GetCurrentWeaponType()
    {
        return weaponHandler.GetWeaponType();
    }


    public Transform GetCurrentWeaponMagazineTransform()
    {
        currentWeaponMagazineTransform = weaponHandler.GetCurrentEquippedMagazine();
        return currentWeaponMagazineTransform;
    }

    public AudioClip GetMagazineRemovedAudioClip()
    {
        return weaponHandler.GetMagazineRemovedAudioClip();
    }

    public AudioClip GetMagazineAttachedAudioClip()
    {
        return weaponHandler.GetMagazineAttachedAudioClip();
    }

    public AudioClip GetReloadLeverPullAudioClip()
    {
        return weaponHandler.GetReloadLeverPullAudioClip();
    }
}
