using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float cameraSpeed = 20f;
    [SerializeField] private float cameraZoomSpeed = 10.0f;
    [SerializeField] private float cameraMaxZoom = 100f;
    [SerializeField] private float cameraMinZoom = 300f;


    private void Awake()
    {
        
    }

    private void Update()
    {
            virtualCamera.transform.position += new Vector3(GameInput.Instance.GetSafeHouseCameraMovementVector().x, 0, GameInput.Instance.GetSafeHouseCameraMovementVector().y) * Time.deltaTime * cameraSpeed;

            if (GameInput.Instance.GetZoom() > 0 && virtualCamera.transform.position.y > cameraMaxZoom)
            {
                virtualCamera.transform.position -= new Vector3(0, cameraZoomSpeed * Time.deltaTime, 0);
            }
            else if (GameInput.Instance.GetZoom() < 0 && virtualCamera.transform.position.y < cameraMinZoom)
            {
                virtualCamera.transform.position += new Vector3(0, cameraZoomSpeed * Time.deltaTime, 0);
            }
    }
}
