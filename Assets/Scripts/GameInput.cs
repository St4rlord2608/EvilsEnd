using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnWeaponChangeButtonPressed;
    public event EventHandler OnShootEnded;
    public event EventHandler OnShootStarted;
    public event EventHandler OnSwitchCombatMode;
    public event EventHandler OnAimStarted;
    public event EventHandler OnSwitchCameraSide;
    public event EventHandler OnPause;
    public event EventHandler OnReload;
    public event EventHandler OnInteract;
    public event EventHandler OnHeal;
    public event EventHandler OnCrouch;

    public event EventHandler OnSafeHouseInteract;
    public event EventHandler OnLeaveInteractView;
    public event EventHandler OnRemoveSafeHouseInteract;
    public event EventHandler OnSwitchToNextInteractabe;
    public event EventHandler OnSwitchToPreviousInteractabe;


    public event EventHandler OnBindingRebind;

    public enum SafeHouseBinding
    {
        Move_Up,
        Move_Down, 
        Move_Left, 
        Move_Right,
        Pause

    }
    public enum PlayerBinding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Pause

    }

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        
        
    }

    private void Start()
    {
        if (GameManager.Instance.GetSceneType() == GameManager.SceneType.SafeHouse)
        {
            playerInputActions.Player.Disable();
            playerInputActions.SafeHouse.Enable();
            playerInputActions.SafeHouse.Interact.performed += SafeHouse_Interact_performed;
            playerInputActions.SafeHouse.LeaveInteractView.performed += LeaveSafeHouseInteractView_performed;
            playerInputActions.SafeHouse.RemoveInteract.performed += RemoveSafeHouseInteract_performed;
            playerInputActions.SafeHouse.SwitchToNextInteractable.performed += SwitchToNextInteractable_performed;
            playerInputActions.SafeHouse.SwitchToPreviousInteractable.performed += SwitchToPreviousInteractable_performed;
            playerInputActions.SafeHouse.Pause.performed += Pause_performed;
        }
        else if (GameManager.Instance.GetSceneType() == GameManager.SceneType.GameScene)
        {
            playerInputActions.SafeHouse.Disable();
            playerInputActions.Player.Enable();
            playerInputActions.Player.ChangeWeapon.performed += ChangeWeapon_performed;
            playerInputActions.Player.Shoot.canceled += Shoot_canceled;
            playerInputActions.Player.Shoot.performed += Shoot_performed;
            playerInputActions.Player.Aim.performed += Aim_performed;
            playerInputActions.Player.SwitchCombatMode.performed += SwitchCombatMode_performed;
            playerInputActions.Player.SwitchCameraSide.performed += SwitchCameraSide_performed;
            playerInputActions.Player.Pause.performed += Pause_performed;
            playerInputActions.Player.Reload.performed += Reload_performed;
            playerInputActions.Player.Interact.performed += Interact_performed;
            playerInputActions.Player.Heal.performed += Heal_performed;
            playerInputActions.Player.Crouch.performed += Crouch_performed;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePause += GameManager_OnGamePause;
            GameManager.Instance.OnGameResume += GameManager_OnGameResume;
        }
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

    private void GameManager_OnGameResume(object sender, EventArgs e)
    {
        playerInputActions.Player.Enable();
    }

    private void GameManager_OnGamePause(object sender, EventArgs e)
    {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Pause.Enable();
    }

    private void Crouch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCrouch?.Invoke(this,EventArgs.Empty);
    }

    private void Heal_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnHeal?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnReload?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void SwitchCameraSide_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwitchCameraSide?.Invoke(this, EventArgs.Empty);
    }

    private void Aim_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAimStarted?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShootStarted?.Invoke(this, EventArgs.Empty);
    }

    private void SwitchCombatMode_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSwitchCombatMode?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShootEnded?.Invoke(this, EventArgs.Empty);
    }

    private void ChangeWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnWeaponChangeButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetCameraMovementVector()
    {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }

    public float GetShoot()
    {
        return playerInputActions.Player.Shoot.ReadValue<float>();
    }

    public float GetAim()
    {
        return playerInputActions.Player.Aim.ReadValue<float>();
    }

    public float GetSprint()
    {
        return playerInputActions.Player.Sprint.ReadValue<float>();
    }

    public string GetSafeHouseBindingText(SafeHouseBinding binding)
    {
        switch (binding)
        {
            case SafeHouseBinding.Move_Up:
                return playerInputActions.SafeHouse.MoveCamera.bindings[1].ToDisplayString();
            case SafeHouseBinding.Pause:
                return playerInputActions.SafeHouse.Pause.bindings[0].ToDisplayString();
        }
        return "";
    }

    public string GetPlayerBindingText(PlayerBinding binding)
    {
        switch (binding)
        {
            case PlayerBinding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case PlayerBinding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
        return "";
    }

    public void RebindSafeHouseBinding(SafeHouseBinding binding, Action onActionRebound)
    {
        playerInputActions.SafeHouse.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case SafeHouseBinding.Move_Up:
                inputAction = playerInputActions.SafeHouse.MoveCamera;
                bindingIndex = 1;
                break;
            case SafeHouseBinding.Move_Down:
                inputAction = playerInputActions.SafeHouse.MoveCamera;
                bindingIndex = 2;
                break;
            case SafeHouseBinding.Move_Left:
                inputAction = playerInputActions.SafeHouse.MoveCamera;
                bindingIndex = 3;
                break;
            case SafeHouseBinding.Move_Right:
                inputAction = playerInputActions.SafeHouse.MoveCamera;
                bindingIndex = 4;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.SafeHouse.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

    public void RebindPlayerBinding(PlayerBinding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case PlayerBinding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case PlayerBinding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case PlayerBinding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case PlayerBinding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
    private void OnDestroy()
    {
        playerInputActions.Player.Disable();
        playerInputActions.SafeHouse.Disable();
    }
}
