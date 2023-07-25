using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingHandler : MonoBehaviour
{
    [SerializeField] private float shootingSpeedTime;
    [SerializeField] private LayerMask shootableLayers;
    [SerializeField] private int enemyLayer = 13;
    [SerializeField] private int leftLegLayer = 7;
    [SerializeField] private int rightLeglayer = 8;
    [SerializeField] private int torsoLayer = 9;
    [SerializeField] private int leftArmLayer = 10;
    [SerializeField] private int rightArmLayer = 11;
    [SerializeField] private int headLayer = 12;
    [SerializeField] private float maxShootingNoise = 10;
    [SerializeField] private TrailRenderer bulletTrail;
    

    private PlayerWeaponHandler playerWeaponHandler;
    private PlayerMovement playerMovement;
    private PlayerAim playerAim;
    private PlayerThirdPersonCameraController playerThirdPersonCameraController;
    private float currentTimeBetweenShot = 0;
    private Transform shootingPoint;
    private Transform gunPivot;
    private float cameraToShootingPointDistance;
    private PlayerReloading playerReloading;
    private int ammunitionInCurrentMagazine;
    private float currentShootingNoise = 0;
    private float currentRecoilPower;
    //private LineRenderer lineRenderer;

    private bool hasShot = false;

    public EventHandler OnShooting;
    public EventHandler OnWeaponEmpty;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerThirdPersonCameraController = GetComponent<PlayerThirdPersonCameraController>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
        playerAim = GetComponent<PlayerAim>();
        playerReloading = GetComponent<PlayerReloading>();
    }

    private void Start()
    {
        shootingPoint = playerWeaponHandler.GetShootingPoint();
        gunPivot = playerWeaponHandler.GetGunPivot();
        currentTimeBetweenShot = shootingSpeedTime;
        ammunitionInCurrentMagazine = playerWeaponHandler.GetWeaponMagazineCapacity();
        //lineRenderer = shootingPoint.GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (playerMovement.isCrouching)
        {
            currentRecoilPower = playerWeaponHandler.GetCrouchingRecoilPower(); ;
        }
        else if (playerMovement.IsSprinting())
        {
            currentRecoilPower = playerWeaponHandler.GetSprintingRecoilPower();
        }
        else
        {
            currentRecoilPower = playerWeaponHandler.GetDefaultRecoilPower();
        }
        ammunitionInCurrentMagazine = playerWeaponHandler.GetAmmunitionInCurrentMagazine();
        Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        cameraToShootingPointDistance = Vector3.Distance(ray.origin, gunPivot.transform.position);
        if (Time.timeScale == 1.0f)
        {
            if (IsShooting() && !playerReloading.GetIsReloading() && ammunitionInCurrentMagazine > 0)
            {
                currentTimeBetweenShot += Time.deltaTime;
                if (currentTimeBetweenShot >= shootingSpeedTime)
                {
                    Shoot();
                    currentTimeBetweenShot = 0;
                }
                else
                {
                    currentShootingNoise = 0;
                }
            }
            else if(IsShooting() && ammunitionInCurrentMagazine <= 0)
            {
                OnWeaponEmpty?.Invoke(this, EventArgs.Empty);
                currentTimeBetweenShot = shootingSpeedTime;
                currentShootingNoise = 0;
            }
            else
            {
                currentTimeBetweenShot = shootingSpeedTime;
                currentShootingNoise = 0;
            }
        }
        
    }

    private void Shoot()
    {
        ammunitionInCurrentMagazine--;
        playerWeaponHandler.SetAmmunitionInCurrentMagazine(ammunitionInCurrentMagazine);
        OnShooting?.Invoke(this, EventArgs.Empty);
        HandleHit();
        currentShootingNoise = maxShootingNoise;
        hasShot = true;
    }

    private void HandleHit()
    {
        Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
        
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        float currentWeaponDamage = playerWeaponHandler.GetCurrentWeaponDamage();
        HandleHitRaycast(ray, currentWeaponDamage);
        
    }

    private void HandleHitRaycast(Ray ray, float currentWeaponDamage)
    {
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, shootableLayers))
        {
            float distance = Vector3.Distance(raycastHit.point, ray.origin);
            if(distance < cameraToShootingPointDistance)
            {
                int previousLayer = raycastHit.transform.gameObject.layer;
                raycastHit.transform.gameObject.layer = 2;
                HandleHitRaycast(ray, currentWeaponDamage);
                raycastHit.transform.gameObject.layer = previousLayer;
            }
            else
            {
                TrailRenderer trail = Instantiate(bulletTrail, shootingPoint.position, Quaternion.identity);
                if (trail.TryGetComponent<BulletTrail>(out BulletTrail bulletTrailScript))
                {
                    bulletTrailScript.SetTarget(raycastHit.point);
                }
                CanBeHit canBeHit = raycastHit.collider.gameObject.GetComponent<CanBeHit>();
                if (canBeHit != null)
                {
                    canBeHit.TakeDamage(raycastHit.point, currentWeaponDamage, transform);
                    if(canBeHit.gameObject.layer == enemyLayer)
                    {
                        HitIndicationUI.Instance.ShowHit();
                    }
                }
                else
                {
                    canBeHit = raycastHit.collider.gameObject.GetComponentInParent<CanBeHit>();
                    if (canBeHit != null)
                    {
                        if (raycastHit.transform.gameObject.layer == headLayer)
                        {
                            canBeHit.HeadDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                        else if (raycastHit.transform.gameObject.layer == leftArmLayer)
                        {
                            canBeHit.LeftArmDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                        else if (raycastHit.transform.gameObject.layer == rightArmLayer)
                        {
                            canBeHit.RightArmDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                        else if (raycastHit.transform.gameObject.layer == torsoLayer)
                        {
                            canBeHit.TorsoDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                        else if (raycastHit.transform.gameObject.layer == leftLegLayer)
                        {
                            canBeHit.LeftLegDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                        else if (raycastHit.transform.gameObject.layer == rightLeglayer)
                        {
                            canBeHit.RightLegDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                        else
                        {
                            canBeHit.TakeDamage(raycastHit.point, currentWeaponDamage, transform);
                        }
                    }
                }
            }
        }
    }

    private bool IsShooting()
    {
        return GameInput.Instance.GetShoot() == 1 && playerAim.GetCombatModeIsActive();
    }

    public float GetCurrentShootingNoise()
    {
        return currentShootingNoise;
    }

    public void SetHasShot(bool hasShot)
    {
        this.hasShot = hasShot;
    }

    public bool GetHasShot()
    {
        return hasShot;
    }

    public float GetRecoilPower()
    {
        return currentRecoilPower;
    }

    public LayerMask GetShootableLayers()
    {
        return shootableLayers;
    }
}
