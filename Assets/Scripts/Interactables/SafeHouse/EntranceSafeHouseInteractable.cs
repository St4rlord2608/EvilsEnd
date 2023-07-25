using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceSafeHouseInteractable : SafeHouseInteractable
{

    [SerializeField] private Transform cameraFocusPoint;

    private Transform focusedVirtualCameraTransform;
    public override void CancelCurrentAction()
    {
        if (focusedVirtualCameraTransform != null)
        {
            focusedVirtualCameraTransform.gameObject.SetActive(false);
            safeHouseController.SetIsInEntrance(false);
            safeHouseController.SetInteractableIsSelected(false);
        }
    }

    public override void Interact(Transform player, bool slowFocusCameraPositioning = false)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        focusedVirtualCameraTransform = safeHouseController.GetFocusCamera();
        focusedVirtualCameraTransform.gameObject.transform.position = cameraFocusPoint.position;
        focusedVirtualCameraTransform.gameObject.transform.rotation = cameraFocusPoint.rotation;
        focusedVirtualCameraTransform.gameObject.SetActive(true);
        safeHouseController.SetIsInEntrance(true);
        safeHouseController.SetInteractableIsSelected(true);
    }
}
