using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    private Transform shootingPoint;

    [SerializeField] private ParticleSystem gunShotFX;
    [SerializeField] private ParticleSystem gunShotSmokeFX;

    private PlayerShootingHandler playerShootingHandler;
    private PlayerWeaponHandler playerWeaponHandler;

    private void Awake()
    {
        playerShootingHandler = GetComponent<PlayerShootingHandler>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
    }
    void Start()
    {
        GameInput.Instance.OnShootEnded += GameInput_OnShootEnded;
        playerShootingHandler.OnShooting += PlayerShootingHandler_OnShooting;
    }

    private void PlayerShootingHandler_OnShooting(object sender, System.EventArgs e)
    {
        shootingPoint = playerWeaponHandler.GetShootingPoint();
        Instantiate(gunShotFX, shootingPoint.position, shootingPoint.rotation, shootingPoint);
    }


    private void GameInput_OnShootEnded(object sender, System.EventArgs e)
    {
        if (playerShootingHandler.GetHasShot())
        {
            shootingPoint = playerWeaponHandler.GetShootingPoint();
            Instantiate(gunShotSmokeFX, shootingPoint.position, shootingPoint.rotation, shootingPoint);
            playerShootingHandler.SetHasShot(false);
        }
    }
}
