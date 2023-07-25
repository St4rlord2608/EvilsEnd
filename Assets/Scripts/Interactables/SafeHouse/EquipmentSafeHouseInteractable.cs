using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSafeHouseInteractable : SafeHouseInteractable
{

    [SerializeField] private Transform cameraFocusPoint;

    private int defaultLayerIndex;
    private Transform focusedVirtualCameraTransform;

    private void Awake()
    {
        defaultLayerIndex = transform.gameObject.layer;
    }

    public override void CancelCurrentAction()
    {
        focusedVirtualCameraTransform.gameObject.SetActive(false);
        safeHouseController.SetInteractableIsSelected(false);
        safeHouseController.SetIsEquipmentSelection(false);
        SafeHouseInteractable currentEquipmentItem = safeHouseController.GetLastEquipmentItemInteractable();
        if (currentEquipmentItem != null)
        {
            currentEquipmentItem.CancelCurrentAction();
        }
        transform.gameObject.layer = defaultLayerIndex;
    }

    public override void Interact(Transform player, bool slowFocusCameraPositioning = false)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        focusedVirtualCameraTransform = safeHouseController.GetFocusCamera();
        if(slowFocusCameraPositioning)
        {
            safeHouseController.StartMovingFocusCameraToInteractable(cameraFocusPoint.position, cameraFocusPoint.rotation);
        }
        else
        {
            focusedVirtualCameraTransform.gameObject.transform.position = cameraFocusPoint.position;
            focusedVirtualCameraTransform.gameObject.transform.rotation = cameraFocusPoint.rotation;
        }
        focusedVirtualCameraTransform.gameObject.SetActive(true);
        safeHouseController.SetInteractableIsSelected(true);
        safeHouseController.SetIsEquipmentSelection(true);
        transform.gameObject.layer = 2;
    }
}
