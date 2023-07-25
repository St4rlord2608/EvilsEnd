using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouseController : MonoBehaviour
{
    [SerializeField] private Transform debugTransform;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private CinemachineVirtualCamera focusedVirtualCamera;

    private SafeHouseInteractable lastSafeHouseInteractable;
    private SafeHouseInteractable lastExitInteractable;
    private SafeHouseInteractable lastEquipmentItemInteractable;
    private bool isInEntrance = false;
    private bool isEquipmentSelection = false;
    private bool interactableIsSelected = false;
    private bool moveFocusCamera = false;

    private Vector3 nextInteractableFocusCameraPosition;
    private Quaternion nextInteractableFocusCameraRotation;

    private void Awake()
    {
        focusedVirtualCamera.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameInput.Instance.OnSafeHouseInteract += GameInput_OnInteract;
        GameInput.Instance.OnLeaveInteractView += GameInput_OnLeaveInteractView;
        GameInput.Instance.OnRemoveSafeHouseInteract += GameInput_OnRemoveInteract;
        GameInput.Instance.OnSwitchToNextInteractabe += GameInput_OnSwitchToNextInteractabe;
        GameInput.Instance.OnSwitchToPreviousInteractabe += GameInput_OnSwitchToPreviousInteractabe;
    }

    private void GameInput_OnSwitchToPreviousInteractabe(object sender, System.EventArgs e)
    {
        if(lastSafeHouseInteractable != null)
        {
            SafeHouseInteractable nextSafeHouseInteractable = lastSafeHouseInteractable.GetNextSafeHouseInteractable();
            if(nextSafeHouseInteractable != null)
            {
                lastSafeHouseInteractable.CancelCurrentAction();
                lastSafeHouseInteractable = nextSafeHouseInteractable;
                nextSafeHouseInteractable.Interact(transform, true);
            }
        }
    }

    private void GameInput_OnSwitchToNextInteractabe(object sender, System.EventArgs e)
    {
        if (lastSafeHouseInteractable != null)
        {
            SafeHouseInteractable previousSafeHouseInteractable = lastSafeHouseInteractable.GetPreviousSafeHouseInteractable();
            if (previousSafeHouseInteractable != null)
            {
                lastSafeHouseInteractable.CancelCurrentAction();
                lastSafeHouseInteractable = previousSafeHouseInteractable;
                previousSafeHouseInteractable.Interact(transform, true);
            }
        }
    }

    private void GameInput_OnRemoveInteract(object sender, System.EventArgs e)
    {
        if (lastSafeHouseInteractable != null && !interactableIsSelected)
        {
            lastSafeHouseInteractable.RemoveInteract(transform);
        }else if(lastEquipmentItemInteractable != null && isEquipmentSelection)
        {
            lastEquipmentItemInteractable.RemoveInteract(transform);
        }
    }

    private void GameInput_OnLeaveInteractView(object sender, System.EventArgs e)
    {
        if (lastSafeHouseInteractable != null)
        {
            lastSafeHouseInteractable.CancelCurrentAction();
        }
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (lastSafeHouseInteractable != null && !interactableIsSelected)
        {
            lastSafeHouseInteractable.SetIsUnselected(transform);
            moveFocusCamera = false;
            lastSafeHouseInteractable.Interact(transform);
        }else if(lastExitInteractable != null && isInEntrance)
        {
            moveFocusCamera = false;
            lastExitInteractable.Interact(transform);
        }else if(lastEquipmentItemInteractable != null && isEquipmentSelection)
        {
            moveFocusCamera = false;
            lastEquipmentItemInteractable.Interact(transform);
        }
    }

    private void Update()
    {
        if(GameManager.Instance.GetIsPause()) return;
        if (moveFocusCamera)
        {
            focusedVirtualCamera.transform.position = Vector3.Lerp(focusedVirtualCamera.transform.position, nextInteractableFocusCameraPosition, Time.deltaTime * 2f);
            focusedVirtualCamera.transform.rotation = Quaternion.Lerp(focusedVirtualCamera.transform.rotation, nextInteractableFocusCameraRotation, Time.deltaTime * 2f);
            if(focusedVirtualCamera.transform.position == nextInteractableFocusCameraPosition && 
                focusedVirtualCamera.transform.rotation == nextInteractableFocusCameraRotation)
            {
                moveFocusCamera = false;
            }
        }
        if(!interactableIsSelected)
        {
            HandleInteractableSelection();
        }else if(isInEntrance)
        {
            HandleExitSelection();
        }else if(isEquipmentSelection)
        {
            HandleEquipmentItemSelection();
        }
    }

    private void HandleInteractableSelection()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, interactableLayerMask))
        {
            if (hit.collider.transform.TryGetComponent<SafeHouseInteractable>(out SafeHouseInteractable safeHouseInteractable))
            {
                if (lastSafeHouseInteractable != null && lastSafeHouseInteractable.gameObject != safeHouseInteractable.gameObject)
                {
                    lastSafeHouseInteractable.SetIsUnselected(transform);
                }
                lastSafeHouseInteractable = safeHouseInteractable;
                lastSafeHouseInteractable.SetIsSelected(transform);
            }
            else
            {
                if (lastSafeHouseInteractable != null)
                {
                    lastSafeHouseInteractable.SetIsUnselected(transform);
                    lastSafeHouseInteractable = null;
                }
            }
        }
        else
        {
            if (lastSafeHouseInteractable != null)
            {
                lastSafeHouseInteractable.SetIsUnselected(transform);
                lastSafeHouseInteractable = null;
            }
        }
    }

    private void HandleExitSelection()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, interactableLayerMask))
        {
            if (hit.collider.transform.TryGetComponent<SafeHouseInteractable>(out SafeHouseInteractable safeHouseInteractable) && safeHouseInteractable.GetIsExit())
            {
                if (lastExitInteractable != null && lastExitInteractable.gameObject != safeHouseInteractable.gameObject)
                {
                    lastExitInteractable.SetIsUnselected(transform);
                }
                lastExitInteractable = safeHouseInteractable;
                lastExitInteractable.SetIsSelected(transform);
            }
            else
            {
                if (lastExitInteractable != null)
                {
                    lastExitInteractable.SetIsUnselected(transform);
                    lastExitInteractable = null;
                }
            }
        }
        else
        {
            if (lastExitInteractable != null)
            {
                lastExitInteractable.SetIsUnselected(transform);
                lastExitInteractable = null;
            }
        }
    }

    private void HandleEquipmentItemSelection()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, interactableLayerMask))
        {
            if (hit.collider.transform.TryGetComponent<SafeHouseInteractable>(out SafeHouseInteractable safeHouseInteractable) && safeHouseInteractable.GetIsEquipmentItem())
            {
                if (lastEquipmentItemInteractable != null && lastEquipmentItemInteractable.gameObject != safeHouseInteractable.gameObject)
                {
                    lastEquipmentItemInteractable.SetIsUnselected(transform);
                }
                lastEquipmentItemInteractable = safeHouseInteractable;
                lastEquipmentItemInteractable.SetIsSelected(transform);
            }
            else
            {
                if (lastEquipmentItemInteractable != null)
                {
                    lastEquipmentItemInteractable.SetIsUnselected(transform);
                    lastEquipmentItemInteractable = null;
                }
            }
        }
        else
        {
            if (lastEquipmentItemInteractable != null)
            {
                lastEquipmentItemInteractable.SetIsUnselected(transform);
                lastEquipmentItemInteractable = null;
            }
        }
    }

    public Transform GetFocusCamera()
    {
        return focusedVirtualCamera.transform;
    }

    public void StartMovingFocusCameraToInteractable(Vector3 postion, Quaternion rotation)
    {
        nextInteractableFocusCameraPosition = postion;
        nextInteractableFocusCameraRotation = rotation;
        moveFocusCamera = true;
    }

    public void SetIsInEntrance(bool isInEntrance)
    {
        this.isInEntrance = isInEntrance;
    }

    public bool GetIsInEntrance()
    {
        return isInEntrance;
    }

    public void SetInteractableIsSelected(bool interactableIsSelected)
    {
        this.interactableIsSelected = interactableIsSelected;
    }

    public bool GetInteractableIsSelected()
    {
        return interactableIsSelected;
    }

    public bool GetIsEquipmentSelection()
    {
        return isEquipmentSelection;
    }

    public void SetIsEquipmentSelection(bool isEquipmentSelection)
    {
        this.isEquipmentSelection = isEquipmentSelection;
    }

    public SafeHouseInteractable GetLastEquipmentItemInteractable()
    {
        return lastEquipmentItemInteractable;
    }

}
