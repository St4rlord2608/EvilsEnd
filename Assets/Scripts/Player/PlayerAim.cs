using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig weaponAimRig;
    [SerializeField] private float rotateCharacterLeftThreshold = 50f;
    [SerializeField] private float rotateCharacterRightThreshold = 30f;
    [SerializeField] private Transform secondHandPositionTarget;
    [SerializeField] private float aimingRotationSpeed;
    [SerializeField] private float changeMovementDirectionSpeed;
    [SerializeField] private float changeRigWeightSpeed = 2.0f;
    [SerializeField] private Transform characterMovementDirectionOrientationTransform;

    public bool isRotatingLeft = false;
    public bool isRotatingRight = false;
    public bool isWalingAimed = false;
    public bool isRunningAimed = false;

    private bool combatModeIsActive = false;

    private PlayerThirdPersonCameraController playerThirdPersonCamera;
    private PlayerWeaponHandler playerWeaponHandler;
    private PlayerMovement playerMovement;
    private PlayerReloading playerReloading;
    private PlayerShootingHandler playerShootingHandler;
    private Vector3 worldAimTarget;
    private Vector3 weaponDirection;
    private Vector3 lookingPosition;
    private float lookingRotation;
    private Vector2 currentMovementVector;
    private Vector2 previousMovementVector;
    private Transform currentWeaponSecondHandPoint;
    private Transform gunPivot;
    private Transform shootingPoint;
    private LayerMask shootableLayers;

    private void Awake()
    {
        playerShootingHandler = GetComponent<PlayerShootingHandler>();
        playerThirdPersonCamera = GetComponent<PlayerThirdPersonCameraController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
        playerReloading = GetComponent<PlayerReloading>();
    }
    private void Start()
    {
        shootableLayers = playerShootingHandler.GetShootableLayers();
        currentWeaponSecondHandPoint = playerWeaponHandler.GetCurrentWeaponSecondHandPoint();
        gunPivot = playerWeaponHandler.GetGunPivot();
        shootingPoint = playerWeaponHandler.GetShootingPoint();
        GameInput.Instance.OnSwitchCombatMode += GameInput_OnSwitchCombatMode;
        GameInput.Instance.OnShootStarted += GameInput_OnShootStarted;
        GameInput.Instance.OnAimStarted += GameInput_OnAimStarted;
    }

    private void GameInput_OnAimStarted(object sender, System.EventArgs e)
    {
        combatModeIsActive = true;
    }

    private void GameInput_OnShootStarted(object sender, System.EventArgs e)
    {
        combatModeIsActive = true;
    }

    private void GameInput_OnSwitchCombatMode(object sender, System.EventArgs e)
    {
        combatModeIsActive = !combatModeIsActive;
    }

    private void Update()
    {
        //animation Rigging position set
        secondHandPositionTarget.position = currentWeaponSecondHandPoint.position;
        secondHandPositionTarget.rotation = currentWeaponSecondHandPoint.rotation;
        Aiming();
        CheckIfIsAimingOnEnemy();
    }

    private void CheckIfIsAimingOnEnemy()
    {
        Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, shootableLayers))
        {
            if (raycastHit.transform.TryGetComponent<Enemy>(out Enemy enemy))
            {
                CrosshairUI.Instance.ShowEnemyAim();
            }
            else
            {
                enemy = raycastHit.transform.GetComponentInParent<Enemy>();
                if(enemy != null)
                {
                    CrosshairUI.Instance.ShowEnemyAim();
                }
                else
                {
                    CrosshairUI.Instance.HideEnemyAim();
                }
            }
        }
    }

    private void Aiming()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, aimColliderLayerMask))
        {
            //aimTarget.position = Vector3.Lerp(aimTarget.position, raycastHit.point, aimingRotationSpeed * Time.deltaTime);
            mouseWorldPosition = raycastHit.point;
        }

        if (combatModeIsActive)
        {
            if(!playerReloading.GetIsReloading())
            {
                if(aimRig.weight < 1.0f)
                {
                    aimRig.weight = Mathf.Lerp(aimRig.weight, 1.0f, changeRigWeightSpeed * Time.deltaTime);
                }
                if(weaponAimRig.weight < 1.0f)
                {
                    weaponAimRig.weight = Mathf.Lerp(weaponAimRig.weight, 1.0f, changeRigWeightSpeed * Time.deltaTime);
                }
                MoveCharacterWhenAngleIsTooHigh();
            }
            else
            {
                lookingPosition = worldAimTarget - transform.position;
                lookingRotation = Mathf.Atan2(lookingPosition.x, lookingPosition.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, lookingRotation + rotateCharacterLeftThreshold, transform.rotation.z)), aimingRotationSpeed * Time.deltaTime);
            }
            playerMovement.isAiming = true;
            if (GameInput.Instance.GetAim() == 1)
            {
                aimVirtualCamera.gameObject.SetActive(true);
            }
            else
            {
                aimVirtualCamera.gameObject.SetActive(false);
                playerThirdPersonCamera.SetMouseSensitivity(normalSensitivity);
            }
            playerThirdPersonCamera.SetMouseSensitivity(aimSensitivity);
            worldAimTarget = aimTarget.position;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            //RotateWeaponToMousePos();
            aimDirection.y = 0;
            if (GameInput.Instance.GetMovementVector() != Vector2.zero)
            {
                HandleMovementDirection();
            }
            else
            {
                isWalingAimed = false;
                isRunningAimed = false;
            }
        }
        else
        {
            weaponAimRig.weight = 0f;
            aimRig.weight = 0f;
            playerMovement.isAiming = false;
            aimVirtualCamera.gameObject.SetActive(false);
            playerThirdPersonCamera.SetMouseSensitivity(normalSensitivity);
            isWalingAimed = false;
            isRunningAimed = false;
        }
    }

    private void RotateWeaponToMousePos()
    {
        /*lookingPosition = worldAimTarget - shootingPoint.position;
        lookingRotation = Mathf.Atan2(lookingPosition.x, lookingPosition.z) * Mathf.Rad2Deg;*/
        weaponDirection = worldAimTarget - shootingPoint.position;
        Quaternion toRotation = Quaternion.LookRotation(weaponDirection);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, toRotation, aimingRotationSpeed * Time.deltaTime);
        //gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(new Vector3(gunPivot.rotation.x, lookingRotation, gunPivot.rotation.z)), 2f * Time.deltaTime);

    }

    private void MoveCharacterWhenAngleIsTooHigh()
    {
        float weaponAndCharacterAngleDifference = ((transform.rotation.eulerAngles.y - gunPivot.transform.rotation.eulerAngles.y) + 360f) % 360;
        lookingPosition = worldAimTarget - transform.position;
        lookingRotation = Mathf.Atan2(lookingPosition.x, lookingPosition.z) * Mathf.Rad2Deg;
        if (weaponAndCharacterAngleDifference < rotateCharacterRightThreshold)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, lookingRotation + rotateCharacterRightThreshold, transform.rotation.z)), aimingRotationSpeed * Time.deltaTime);
        }
        if (weaponAndCharacterAngleDifference > rotateCharacterLeftThreshold)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, lookingRotation + rotateCharacterLeftThreshold, transform.rotation.z)), aimingRotationSpeed * Time.deltaTime);
        }
        isRotatingRight = false;
        isRotatingLeft = false;
    }

    private void HandleMovementDirection()
    {
        isWalingAimed = true;
        Vector3 shootingPointForward = transform.InverseTransformDirection(new Vector3(characterMovementDirectionOrientationTransform.forward.x, 0f, characterMovementDirectionOrientationTransform.forward.z));
        //Vector3 shootingPointForward = transform.InverseTransformDirection(new Vector3(shootingPoint.forward.x, 0f, shootingPoint.forward.z));
        float shootingPointDegree = Mathf.Atan2(shootingPointForward.x, shootingPointForward.z) * Mathf.Rad2Deg;
        float inputDegree = Mathf.Atan2(GameInput.Instance.GetMovementVector().x, GameInput.Instance.GetMovementVector().y) * Mathf.Rad2Deg;
        float movementDegree = shootingPointDegree + inputDegree;
        float movementRadians = movementDegree * (Mathf.PI / 180);
        Vector2 movementDegreeVector = new Vector2(Mathf.Sin(movementRadians), Mathf.Cos(movementRadians));
        if(playerMovement.IsSprinting())
        {
            isRunningAimed = true;
        }
        else
        {
            isRunningAimed = false;
        }
        currentMovementVector = Vector2.Lerp(previousMovementVector, movementDegreeVector, changeMovementDirectionSpeed * Time.deltaTime);
        previousMovementVector = currentMovementVector;
    }

    public Vector2 GetCurrentMovementVector()
    {
       
        return currentMovementVector;
    }

    public bool GetCombatModeIsActive()
    {
        return combatModeIsActive;
    }

}
