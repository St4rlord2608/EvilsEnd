using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerReloading : MonoBehaviour
{
    [SerializeField] private Rig weaponAimRig;
    [SerializeField] private Rig secondHandRig;
    [SerializeField] private Transform reloadHand;
    [SerializeField] private float MaxTimeBetweenReloads = 1f;

    private float currentTimeBetweenReload;
    private string weaponType;
    private Transform instantiatedMagazine;
    private Transform magazineInWeaponTransform;

    private bool isReloading = false;
    private bool bulletInChamber = false;

    private AnimationEventHandler animationEventHandler;
    private PlayerWeaponHandler playerWeaponHandler;
    private PlayerShootingHandler playerShootingHandler;
    private PlayerInventory playerInventory;

    public event EventHandler<ReloadEventArgs> OnReload;
    public event EventHandler OnMagazineRemoved;
    public event EventHandler OnMagazineAttached;

    public class ReloadEventArgs : EventArgs
    {
        public bool bulletInChamber;
    }

    private void Awake()
    {
        animationEventHandler = GetComponent<AnimationEventHandler>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
        playerShootingHandler = GetComponent<PlayerShootingHandler>();
        playerInventory = GetComponent<PlayerInventory>();
        weaponType = playerWeaponHandler.GetCurrentWeaponType();
    }

    private void Start()
    {
        GameInput.Instance.OnReload += GameInput_OnReload;
        animationEventHandler.OnFinish += AnimationEventHandler_OnFinish;
        animationEventHandler.OnMagazineAttached += AnimationEventHandler_OnMagazineAttached;
        animationEventHandler.OnMagazineRemoved += AnimationEventHandler_OnMagazineRemoved;
        playerShootingHandler.OnWeaponEmpty += PlayerShootingHandler_OnWeaponEmpty;
    }

    private void PlayerShootingHandler_OnWeaponEmpty(object sender, EventArgs e)
    {
        HandleReloading();
    }

    private void AnimationEventHandler_OnMagazineRemoved(object sender, EventArgs e)
    {
        magazineInWeaponTransform = playerWeaponHandler.GetCurrentWeaponMagazineTransform();
        instantiatedMagazine = Instantiate(magazineInWeaponTransform, reloadHand);
        instantiatedMagazine.position = magazineInWeaponTransform.position;
        instantiatedMagazine.rotation = magazineInWeaponTransform.rotation;
        OnMagazineRemoved?.Invoke(this, EventArgs.Empty);
    }

    private void AnimationEventHandler_OnMagazineAttached(object sender, EventArgs e)
    {
        Destroy(instantiatedMagazine.gameObject);
        OnMagazineAttached?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        weaponType = playerWeaponHandler.GetCurrentWeaponType();
        if (!isReloading)
        {
            currentTimeBetweenReload += Time.deltaTime;
            secondHandRig.weight = 1.0f;
        }
        else
        {
            currentTimeBetweenReload = 0f;
        }
    }

    private void AnimationEventHandler_OnFinish(object sender, System.EventArgs e)
    {
        HandleAmmunitionInCurrentMagazine();
    }

    private void GameInput_OnReload(object sender, System.EventArgs e)
    {
        HandleReloading();
    }

    private void HandleReloading()
    {
        if (!isReloading && currentTimeBetweenReload >= MaxTimeBetweenReloads)
        {
            if(playerInventory.GetAmountFromInventory(weaponType) > 0)
            {
                weaponAimRig.weight = 0.0f;
                secondHandRig.weight = 0.0f;
                
                if(playerWeaponHandler.GetAmmunitionInCurrentMagazine() > 0)
                {
                    bulletInChamber = true;
                    playerInventory.AddToInventory(weaponType, playerWeaponHandler.GetAmmunitionInCurrentMagazine() - 1);
                    playerWeaponHandler.SetAmmunitionInCurrentMagazine(1);
                }
                else
                {
                    bulletInChamber = false;
                    playerInventory.AddToInventory(weaponType, playerWeaponHandler.GetAmmunitionInCurrentMagazine());
                    playerWeaponHandler.SetAmmunitionInCurrentMagazine(0);
                }
                isReloading = true;
                OnReload?.Invoke(this, new ReloadEventArgs()
                {
                    bulletInChamber = bulletInChamber
                });
            }
        }
    }

    public void HandleAmmunitionInCurrentMagazine()
    {
        int magazineCapacity = playerWeaponHandler.GetWeaponMagazineCapacity();
        int ammunitionForWeaponType = (int)playerInventory.GetAmountFromInventory(weaponType);
        isReloading = false;
        int newAmmunitionInCurrentMagazine;
        if (ammunitionForWeaponType - magazineCapacity < 0)
        {
            newAmmunitionInCurrentMagazine = magazineCapacity - Mathf.Abs(ammunitionForWeaponType - magazineCapacity);
            playerInventory.RemoveFromInventory(weaponType, ammunitionForWeaponType);
        }
        else
        {
            newAmmunitionInCurrentMagazine = magazineCapacity;
            playerInventory.RemoveFromInventory(weaponType, newAmmunitionInCurrentMagazine);
        }
        playerWeaponHandler.SetAmmunitionInCurrentMagazine(playerWeaponHandler.GetAmmunitionInCurrentMagazine() + newAmmunitionInCurrentMagazine);
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }
}
