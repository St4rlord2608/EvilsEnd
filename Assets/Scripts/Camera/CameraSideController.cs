using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSideController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cinemachineThirdPersonCameras;
    [SerializeField] private float switchCameraSideSpeed = 1.0f;

    private bool cameraIsLeft = false;


    private void Start()
    {
        GameInput.Instance.OnSwitchCameraSide += GameInput_OnSwitchCameraSide;
    }
    void Update()
    {
        if (!cameraIsLeft)
        {
            foreach (CinemachineVirtualCamera camera in cinemachineThirdPersonCameras)
            {
                var cameraComponent = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                if (cameraComponent.CameraSide < 1.0f)
                {
                    cameraComponent.CameraSide += Time.deltaTime * switchCameraSideSpeed;
                }

            }
        }
        else
        {
            foreach (CinemachineVirtualCamera camera in cinemachineThirdPersonCameras)
            {
                var cameraComponent = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                if (cameraComponent.CameraSide > 0.0f)
                {
                    cameraComponent.CameraSide -= Time.deltaTime * switchCameraSideSpeed;
                }

            }
        }
    }

    private void GameInput_OnSwitchCameraSide(object sender, System.EventArgs e)
    {
        cameraIsLeft = !cameraIsLeft;
    }
}
