using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private const string SHOOTING_POINT = "ShootingPoint";
    private const string SECOND_HAND_POINT = "SecondHandPosition";
    private const string ENEMY_TAG = "Enemy";
    private const string STONE_GROUND_TAG = "StoneGround";

    private Vector3 worldPosition;
    private Vector3 direction;
    private Vector3 previousDirection;
    private float movingAngle;
    private Transform shootingPoint;
    private Transform gunPivot;
    private LineRenderer aimLine;
    private LineRenderer objectInBetweenLine;
    private Transform currentWeaponSecondHandPoint;
    private int currentWeaponIndex;
    private List<Transform> InstantiatedWeapons;
    private float previousShotTime;
    private Vector3 weaponDirection;
    private float currentPlayerNoise;
    private float currentMoveNoise;
    private float currentShotNoise;

    [SerializeField] private float moveNoise;
    [SerializeField] private float shotNoise;
    [SerializeField] private Transform secondHandPositionTarget;
    [SerializeField] private Transform currentWeapon;
    [SerializeField] private LayerMask mouseDetectionLayer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform mousePositionTransform;
    [SerializeField] private float forwardThreshold = 45f;
    [SerializeField] private float sideWayThreshold = 135f;
    [SerializeField] private float backwardThreshold = 225f;
    [SerializeField] private float rotateCharacterLeftThreshold;
    [SerializeField] private float rotateCharacterRightThreshold;
    [SerializeField] private Transform gunHandTransform;
    [SerializeField] private Transform[] availableWeapons;
    private Rigidbody rb;
    [SerializeField] private Transform cameraFocus;
    [SerializeField] private float shootingSpeedTime;

    public bool isWalkingForward = false;
    public bool isWalkingLeft = false;
    public bool isWalkingRight = false;
    public bool isWalkingBackwards = false;
    public bool isRotatingLeft = false;
    public bool isRotatingRight = false;

    //public event EventHandler OnRotate;
    //public event EventHandler<OnEnemyHitEventArgs> OnEnemyHit;
    //public event EventHandler<OnStoneGroundHitEventArgs> OnStoneGroundHit;
    public event EventHandler OnShooting;

    public class OnEnemyHitEventArgs : EventArgs
    {
        public Vector3 hitPosition;
    }
    public class OnStoneGroundHitEventArgs : EventArgs
    {
        public Vector3 hitPosition;
    }


    private void Awake()
    {
        InstantiatedWeapons = new List<Transform>();
        currentWeaponIndex = 0;
        InstantiateAvailableWeapons();
    }

    private void InstantiateAvailableWeapons()
    {
        foreach (Transform weapon in availableWeapons)
        {
            currentWeapon = Instantiate(weapon, gunHandTransform.position, gunHandTransform.rotation, gunHandTransform);
            InstantiatedWeapons.Add(currentWeapon);
            currentWeapon.gameObject.SetActive(false);
        }
        currentWeapon = InstantiatedWeapons[0];
        currentWeapon.gameObject.SetActive(true);
        ActivateNewWeapon();
    }

    private void ActivateNewWeapon()
    {
        if (!currentWeapon.gameObject.activeSelf)
        {
            if (currentWeaponIndex + 1 == InstantiatedWeapons.Count())
            {
                currentWeaponIndex = 0;
            }
            else
            {
                currentWeaponIndex++;
            }
            currentWeapon = InstantiatedWeapons[currentWeaponIndex];
            currentWeapon.gameObject.SetActive(true);
        }
        currentWeaponSecondHandPoint = currentWeapon.Find(SECOND_HAND_POINT);
        gunPivot = currentWeapon;
        shootingPoint = currentWeapon.Find(SHOOTING_POINT);
        aimLine = shootingPoint.GetComponent<LineRenderer>();
        objectInBetweenLine = shootingPoint.GetChild(0).GetComponent<LineRenderer>();
    }

    void Start()
    {

        previousShotTime = shootingSpeedTime;
        rb = GetComponent<Rigidbody>();
        GameInput.Instance.OnWeaponChangeButtonPressed += GameInput_OnWeaponChangeButtonPressed;
    }

    private void GameInput_OnWeaponChangeButtonPressed(object sender, EventArgs e)
    {
        currentWeapon.gameObject.SetActive(false);
        ActivateNewWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UnityEngine.Cursor.visible = false;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            UnityEngine.Cursor.visible = true;
        }
        Move();
        HandleOrientation();
        HandleCameraFocus();
        AimLineHandler();
        Shoot();
    }

    private void Move()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        movingAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        if (moveDir.x != 0 || moveDir.z != 0)
        {
            currentMoveNoise = moveNoise;
            HandleMoveDirection();
        }
        else
        {
            currentMoveNoise = 0;
            SetMovementDirectionBools(false, false, false, false);
        }
    }

    private void HandleOrientation()
    {
        GetMousePos();
        RotateWeaponToMousePos();
        RotateToMousePos();
    }
    private void GetMousePos()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
        {
            worldPosition = hit.point;
            direction = hit.point - transform.position;
        }
        mousePositionTransform.position = worldPosition;
    }

    private void RotateWeaponToMousePos()
    {
        weaponDirection = worldPosition - shootingPoint.position;
        Quaternion toRotation = Quaternion.LookRotation(weaponDirection);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, toRotation, rotationSpeed * Time.deltaTime);

        //animation Rigging position set
        secondHandPositionTarget.position = currentWeaponSecondHandPoint.position;
        secondHandPositionTarget.rotation = currentWeaponSecondHandPoint.rotation;
    }

    private void RotateToMousePos()
    {
        direction.y = 0f;
        if (previousDirection != direction && MoveCharacterWhenAngleIsTooHigh())
        {
            RotateCharacter();
        }
        previousDirection = direction;
    }

    private void HandleCameraFocus()
    {
        if (direction.x < 5f && direction.x > -5f)
        {
            cameraFocus.position = new Vector3(transform.position.x + direction.x / 2, cameraFocus.position.y, cameraFocus.position.z);
        }
        else
        {
            if (direction.x > 5f)
            {
                cameraFocus.position = new Vector3(transform.position.x + 2.5f, cameraFocus.position.y, cameraFocus.position.z);
            }
            if (direction.x < -5f)
            {
                cameraFocus.position = new Vector3(transform.position.x - 2.5f, cameraFocus.position.y, cameraFocus.position.z);
            }
        }
        if (direction.z < 5f && direction.z > -5f)
        {
            cameraFocus.position = new Vector3(cameraFocus.position.x, cameraFocus.position.y, transform.position.z + direction.z / 2);
        }
        else
        {
            if (direction.z > 5f)
            {
                cameraFocus.position = new Vector3(cameraFocus.position.x, cameraFocus.position.y, transform.position.z + 2.5f);
            }
            if (direction.z < -5f)
            {
                cameraFocus.position = new Vector3(cameraFocus.position.x, cameraFocus.position.y, transform.position.z - 2.5f); ;
            }
        }
    }

    private void AimLineHandler()
    {
        aimLine.SetPosition(0, shootingPoint.position);
        aimLine.SetPosition(1, worldPosition);

        //stops at object, which are in between player and mousePosition
        if (Physics.Raycast(shootingPoint.position, weaponDirection, out RaycastHit hit, Mathf.Infinity))
        {
            objectInBetweenLine.enabled = true;
            objectInBetweenLine.SetPosition(0, shootingPoint.position);
            objectInBetweenLine.SetPosition(1, hit.point);
        }
        else
        {
            objectInBetweenLine.enabled = false;
        }
    }

    private void Shoot()
    {
        if (GameInput.Instance.GetShoot() == 1)
        {
            previousShotTime += Time.deltaTime;
            if (previousShotTime >= shootingSpeedTime)
            {
                currentShotNoise = shotNoise;
                OnShooting?.Invoke(this, EventArgs.Empty);
                if (Physics.Raycast(shootingPoint.position, weaponDirection, out RaycastHit hit, Mathf.Infinity))
                {
                    CanBeHit canBeHit = hit.transform.GetComponent<CanBeHit>();
                    if (canBeHit != null)
                    {
                        canBeHit.TakeDamage(hit.point, 20, transform);
                    }
                }
                previousShotTime = 0;
            }
            else
            {
                currentShotNoise = 0;
            }
        }
        else
        {
            currentShotNoise = 0;
            previousShotTime = shootingSpeedTime;
        }

    }

    private void RotateCharacter()
    {
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

    }

    private bool MoveCharacterWhenAngleIsTooHigh()
    {
        float weaponAndCharacterAngleDifference = ((transform.rotation.eulerAngles.y - gunPivot.transform.rotation.eulerAngles.y) + 360f) % 360;
        if (weaponAndCharacterAngleDifference > rotateCharacterLeftThreshold && weaponAndCharacterAngleDifference <= 360f - rotateCharacterRightThreshold)
        {
            if (weaponAndCharacterAngleDifference < rotateCharacterLeftThreshold)
            {
                isRotatingLeft = false;
                isRotatingRight = true;
            }
            else
            {
                isRotatingRight = false;
                isRotatingLeft = true;
            }
            return true;
        }
        isRotatingRight = false;
        isRotatingLeft = false;
        return false;
    }

    private void HandleMoveDirection()
    {
        if (movingAngle < 0f)
        {
            movingAngle = 360f - Mathf.Abs(movingAngle);
        }
        float angleDifference = ((transform.rotation.eulerAngles.y - movingAngle) + 360f) % 360;

        if (angleDifference < forwardThreshold || angleDifference > 360f - forwardThreshold)
        {
            SetMovementDirectionBools(false, true, false, false);
        }
        else if (angleDifference >= forwardThreshold && angleDifference < sideWayThreshold)
        {
            SetMovementDirectionBools(false, false, false, true);
        }
        else if (angleDifference < 360f - forwardThreshold && angleDifference > 360 - sideWayThreshold)
        {
            SetMovementDirectionBools(false, false, true, false);
        }
        else if (angleDifference >= sideWayThreshold && angleDifference < backwardThreshold)
        {
            SetMovementDirectionBools(true, false, false, false);
        }
    }

    private void SetMovementDirectionBools(bool back, bool forward, bool right, bool left)
    {
        isWalkingBackwards = back;
        isWalkingForward = forward;
        isWalkingRight = right;
        isWalkingLeft = left;
    }


    public Transform GetCurrentWeaponShootingPoint()
    {
        return shootingPoint;
    }

    public float GetCurrentPlayerNoise()
    {
        if (currentShotNoise >= currentMoveNoise)
        {
            return currentShotNoise;
        }
        else
        {
            return currentMoveNoise;
        }
    }
}
