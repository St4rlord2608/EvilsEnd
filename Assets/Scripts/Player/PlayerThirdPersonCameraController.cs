using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThirdPersonCameraController : MonoBehaviour
{
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject CinemachineCameraTarget;

    [Tooltip("For locking the camera position on all axis")]
    [SerializeField] private bool LockCameraPosition = false;

    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] private float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] private float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField] private float CameraAngleOverride = 0.0f;


    // cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float MouseSensitivity = 1f;
    
    private PlayerShootingHandler playerShootingHandler;


    private const float threshold = 0.01f;


    private void Awake()
    {
        playerShootingHandler = GetComponent<PlayerShootingHandler>();
        playerShootingHandler.OnShooting += PlayerShootingHandler_OnShooting;
    }

    private void PlayerShootingHandler_OnShooting(object sender, System.EventArgs e)
    {
        CameraRotation(true);
    }

    private void Start()
    {
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }


    void LateUpdate()
    {
        if(Time.timeScale == 1.0f)
        {
            CameraRotation();
        }
    }

    private void CameraRotation(bool recoil = false)
    {
        // if there is an input and camera position is not fixed
        if (GameInput.Instance.GetCameraMovementVector().sqrMagnitude >= threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1.0f;

            cinemachineTargetYaw += GameInput.Instance.GetCameraMovementVector().x * deltaTimeMultiplier * MouseSensitivity;
            cinemachineTargetPitch -= GameInput.Instance.GetCameraMovementVector().y * deltaTimeMultiplier * MouseSensitivity;
        }
        if (recoil)
        {
            cinemachineTargetYaw += Random.Range(playerShootingHandler.GetRecoilPower(), -playerShootingHandler.GetRecoilPower());
            cinemachineTargetPitch -= playerShootingHandler.GetRecoilPower();
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SetMouseSensitivity(float newMouseSensitivity)
    {
        MouseSensitivity = newMouseSensitivity;
    }

    public Transform GetCinemachineCameraTarget()
    {
        return CinemachineCameraTarget.transform;
    }
}
