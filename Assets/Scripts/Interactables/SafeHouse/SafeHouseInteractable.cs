using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SafeHouseInteractable : MonoBehaviour
{
    [SerializeField] protected Transform selectedTransform;
    [SerializeField] private SafeHouseInteractable nextSafeHouseInteractable;
    [SerializeField] private SafeHouseInteractable previousSafeHouseInteractable;
    [SerializeField] protected SafeHouseInteractableScriptableObject interactableData;

    protected SafeHouseController safeHouseController;
    protected bool isExit = false;
    protected bool isEquipmentItem = false;

    private void Awake()
    {
        selectedTransform.gameObject.SetActive(false);
    }


    public virtual void SetIsSelected(Transform player)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        if(!safeHouseController.GetInteractableIsSelected())
        {
            if(interactableData != null)
            {
                InteractActionUI.Instance.SetTextAndActivate(GetSelectedDisplayHint());
            }
            selectedTransform.gameObject.SetActive(true);
        }
    }
    public virtual void SetIsUnselected(Transform player)
    {
        InteractActionUI.Instance.Hide();
        selectedTransform.gameObject.SetActive(false);
    }

    public abstract void Interact(Transform player, bool slowFocusCameraPositioning = false);

    public virtual void RemoveInteract(Transform player)
    {

    }

    public abstract void CancelCurrentAction();

    public bool GetIsExit()
    {
        return isExit;
    }

    public bool GetIsEquipmentItem()
    {
        return isEquipmentItem;
    }

    public SafeHouseInteractable GetNextSafeHouseInteractable()
    {
        if(nextSafeHouseInteractable == null)
        {
            return this;
        }
        return nextSafeHouseInteractable;
    }

    public SafeHouseInteractable GetPreviousSafeHouseInteractable()
    {
        if (previousSafeHouseInteractable == null)
        {
            return this;
        }
        return previousSafeHouseInteractable;
    }

    public string GetSelectedDisplayHint()
    {
        return interactableData.interactionDisplayText;
    }
    
}
