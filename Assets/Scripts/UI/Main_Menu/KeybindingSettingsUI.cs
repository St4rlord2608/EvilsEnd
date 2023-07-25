using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeybindingSettingsUI : MonoBehaviour
{
    [SerializeField] private Button safeHouseMoveUpButton;
    [SerializeField] private Button safeHousePauseButton;
    [Space]
    [SerializeField] private TextMeshProUGUI safeHouseCameraMoveUpText;
    [SerializeField] private TextMeshProUGUI safeHousePauseText;
    [Space]
    [SerializeField] private Transform pressToRebindKeyTransform;

    private void Awake()
    {
        safeHouseMoveUpButton.onClick.AddListener(() =>
        {
            RebindSafeHouseBinding(GameInput.SafeHouseBinding.Move_Up);
        });
    }
    private void Start()
    {
        UpdateVisuals();
        HidePressToRebindKey();
    }

    private void UpdateVisuals()
    {
        safeHouseCameraMoveUpText.text = GameInput.Instance.GetSafeHouseBindingText(GameInput.SafeHouseBinding.Move_Up);
        safeHousePauseText.text = GameInput.Instance.GetSafeHouseBindingText(GameInput.SafeHouseBinding.Pause);
    }

    private void RebindSafeHouseBinding(GameInput.SafeHouseBinding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindSafeHouseBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisuals();
        });
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
}
