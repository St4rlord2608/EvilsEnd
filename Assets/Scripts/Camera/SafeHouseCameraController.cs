using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouseCameraController : MonoBehaviour
{


    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float cameraSpeed = 20f;
    [SerializeField] private float cameraZoomSpeed = 10.0f;
    [SerializeField] private float cameraMaxZoom = 5.0f;
    [SerializeField] private float cameraMinZoom = 20.0f;

    private SafeHouseController safeHouseController;

    private void Awake()
    {
        safeHouseController = GetComponent<SafeHouseController>();
    }

    private void Update()
    {
        if (!safeHouseController.GetInteractableIsSelected())
        {
            virtualCamera.transform.position += new Vector3(0, GameInput.Instance.GetSafeHouseCameraMovementVector().y, GameInput.Instance.GetSafeHouseCameraMovementVector().x) * Time.deltaTime * cameraSpeed;

            if (GameInput.Instance.GetZoom() > 0 && virtualCamera.transform.position.x > cameraMaxZoom)
            {
                virtualCamera.transform.position -= new Vector3(cameraZoomSpeed * Time.deltaTime, 0, 0);
            }
            else if (GameInput.Instance.GetZoom() < 0 && virtualCamera.transform.position.x < cameraMinZoom)
            {
                virtualCamera.transform.position += new Vector3(cameraZoomSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
