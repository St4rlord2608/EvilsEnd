using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouseGameInput : MonoBehaviour
{
    public static SafeHouseGameInput Instance;

    private PlayerInputActions playerInputActions;

    public event EventHandler OnSafeHouseInteract;
    public event EventHandler OnLeaveInteractView;
    public event EventHandler OnRemoveSafeHouseInteract;
    public event EventHandler OnSwitchToNextInteractabe;
    public event EventHandler OnSwitchToPreviousInteractabe;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Disable();
        playerInputActions.SafeHouse.Enable();
        playerInputActions.SafeHouse.Interact.performed += SafeHouse_Interact_performed;
        playerInputActions.SafeHouse.LeaveInteractView.performed += LeaveSafeHouseInteractView_performed;
        playerInputActions.SafeHouse.RemoveInteract.performed += RemoveSafeHouseInteract_performed;
        playerInputActions.SafeHouse.SwitchToNextInteractable.performed += SwitchToNextInteractable_performed;
        playerInputActions.SafeHouse.SwitchToPreviousInteractable.performed += SwitchToPreviousInteractable_performed;
    }

    private void SwitchToPreviousInteractable_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwitchToPreviousInteractabe?.Invoke(this, EventArgs.Empty);
    }

    private void SwitchToNextInteractable_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwitchToNextInteractabe?.Invoke(this, EventArgs.Empty);
    }

    private void RemoveSafeHouseInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRemoveSafeHouseInteract?.Invoke(this, EventArgs.Empty);
    }

    private void LeaveSafeHouseInteractView_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnLeaveInteractView?.Invoke(this, EventArgs.Empty);
    }

    private void SafeHouse_Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSafeHouseInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetSafeHouseCameraMovementVector()
    {
        return playerInputActions.SafeHouse.MoveCamera.ReadValue<Vector2>();
    }

    public float GetZoom()
    {
        return playerInputActions.SafeHouse.Zoom.ReadValue<Vector2>().y;
    }

    private void OnDestroy()
    {
        playerInputActions.SafeHouse.Disable();
    }
}
