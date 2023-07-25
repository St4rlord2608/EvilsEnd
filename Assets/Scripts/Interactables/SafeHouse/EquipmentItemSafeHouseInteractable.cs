using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItemSafeHouseInteractable : SafeHouseInteractable
{
    [SerializeField] private SelectedAmountUI selectedAmountUI;
    [SerializeField] private int maxChangeAmount = 1;

    private void Awake()
    {
        isEquipmentItem = true;
    }
    public override void SetIsSelected(Transform player)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        if (safeHouseController.GetIsEquipmentSelection())
        {
            InteractActionUI.Instance.SetTextAndActivate(GetSelectedDisplayHint());
            selectedTransform.gameObject.SetActive(true);
        }
    }
    public override void SetIsUnselected(Transform player)
    {
        if (SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type) <= 0)
        {
            InteractActionUI.Instance.Hide();
            selectedTransform.gameObject.SetActive(false);
        }
    }
    public override void CancelCurrentAction()
    {
        if (SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type) <= 0)
        {
            selectedTransform.gameObject.SetActive(false);
            if (selectedAmountUI != null)
            {
                selectedAmountUI.Hide();
            }
        }
    }

    public override void Interact(Transform player, bool slowFocusCameraPositioning = false)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        if (safeHouseController.GetIsEquipmentSelection())
        {
            int storageValue = (int)SafeHouseInventoryManager.Instance.GetAmountFromInventory(interactableData.type);
            int changeAmount = maxChangeAmount;
            if(storageValue < maxChangeAmount)
            {
                changeAmount = storageValue;
            }
            SafeHouseInventoryManager.Instance.RemoveFromInventory(interactableData.type, changeAmount);
            SafeHouseInventoryManager.Instance.AddToExploreInventory(interactableData.type, changeAmount);
            if(selectedAmountUI != null)
            {
                selectedAmountUI.SetAmount((int)SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type));
                selectedAmountUI.Show();
            }
        }
    }

    public override void RemoveInteract(Transform player)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        if (safeHouseController.GetIsEquipmentSelection())
        {
            if(SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type) > 0)
            {
                int exportValue = (int)SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type);
                int changeAmount = maxChangeAmount;
                if (exportValue < maxChangeAmount)
                {
                    changeAmount = exportValue;
                }
                SafeHouseInventoryManager.Instance.RemoveFromExploreInventory(interactableData.type, changeAmount);
                SafeHouseInventoryManager.Instance.AddToInventory(interactableData.type, changeAmount);
            }
            if(selectedAmountUI != null)
            {
                selectedAmountUI.SetAmount((int)SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type));
                if(SafeHouseInventoryManager.Instance.GetAmountFromExploreInventory(interactableData.type) <= 0)
                {
                    //selectedAmountUI.Hide();
                }
            }
        }
    }
}
