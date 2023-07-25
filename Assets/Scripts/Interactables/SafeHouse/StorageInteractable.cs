using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInteractable : SafeHouseInteractable
{
    [SerializeField] private Transform cameraFocusPoint;
    [SerializeField] private InteractableInventoryCountUI interactableInventoryCountUI;
    [SerializeField] private Transform[] materialVisualSpawnPoints;
    [SerializeField] private Transform[] visualTransformPrefabs;
    [SerializeField] private int materialAmountUntilNextVisualStage = 5;

    private int maxMaterialAmount;

    private Transform focusedVirtualCameraTransform;

    private void Awake()
    {
        maxMaterialAmount = visualTransformPrefabs.Length * materialVisualSpawnPoints.Length * materialAmountUntilNextVisualStage;
        if (interactableInventoryCountUI != null)
        {
            interactableInventoryCountUI.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        SpawnMaterialVisual();
    }

    private void SpawnMaterialVisual()
    {
        int visualStageCount = (int)SafeHouseInventoryManager.Instance.GetAmountFromInventory(interactableData.type) / materialAmountUntilNextVisualStage;
        if (maxMaterialAmount < (int)SafeHouseInventoryManager.Instance.GetAmountFromInventory(interactableData.type))
        {
            visualStageCount = visualTransformPrefabs.Length * materialVisualSpawnPoints.Length;
        }
        int fullVisualCount = visualStageCount / visualTransformPrefabs.Length;
        int spawnedFullVisuals = 0;
        for (; spawnedFullVisuals < fullVisualCount; spawnedFullVisuals++)
        {
            Instantiate(visualTransformPrefabs[visualTransformPrefabs.Length - 1], materialVisualSpawnPoints[spawnedFullVisuals].position,
                materialVisualSpawnPoints[spawnedFullVisuals].rotation);
        }
        int spawnedVisualCount = fullVisualCount * visualTransformPrefabs.Length;
        if (visualStageCount - spawnedVisualCount > 0)
        {
            Instantiate(visualTransformPrefabs[(visualStageCount - spawnedVisualCount) - 1], materialVisualSpawnPoints[spawnedFullVisuals].position,
                 materialVisualSpawnPoints[spawnedFullVisuals].rotation);
        }
    }

    public override void Interact(Transform player, bool slowFocusCameraPositioning = false)
    {
        if (interactableInventoryCountUI != null)
        {
            safeHouseController = player.GetComponent<SafeHouseController>();
            focusedVirtualCameraTransform = safeHouseController.GetFocusCamera();
            safeHouseController.StartMovingFocusCameraToInteractable(cameraFocusPoint.position, cameraFocusPoint.rotation);
            focusedVirtualCameraTransform.gameObject.transform.position = cameraFocusPoint.position;
            focusedVirtualCameraTransform.gameObject.transform.rotation = cameraFocusPoint.rotation;
            focusedVirtualCameraTransform.gameObject.SetActive(true);
            interactableInventoryCountUI.SetTypeText(interactableData.type);
            interactableInventoryCountUI.SetAmountText(SafeHouseInventoryManager.Instance.GetAmountFromInventory(interactableData.type).ToString());
            interactableInventoryCountUI.gameObject.SetActive(true);
            safeHouseController.SetInteractableIsSelected(true);
        }
    }

    public override void CancelCurrentAction()
    {
        if (interactableInventoryCountUI != null && focusedVirtualCameraTransform != null)
        {
            focusedVirtualCameraTransform.gameObject.SetActive(false);
            interactableInventoryCountUI.gameObject.SetActive(false);
            safeHouseController.SetInteractableIsSelected(false);
        }
    }
}
