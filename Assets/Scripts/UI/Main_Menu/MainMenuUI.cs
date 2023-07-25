using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance;
    [SerializeField] private Transform loadVirtualCameraTransform;
    [SerializeField] private Transform settingsVirtualCameraTranform;
    [SerializeField] private Transform newVirtualCameraTransform;
    [Space]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Instance = this;
        loadButton.onClick.AddListener(() =>
        {
            loadVirtualCameraTransform.gameObject.SetActive(true);
            LoadUI.Instance.Show();
            Hide();
        });
        newGameButton.onClick.AddListener(() =>
        {
            newVirtualCameraTransform.gameObject.SetActive(true);
            NewGameUI.Instance.Show();
            Hide();
        });
        settingsButton.onClick.AddListener(() =>
        {
            settingsVirtualCameraTranform.gameObject.SetActive(true);
            MainMenuSettingsUI.Instance.Show();
            Hide();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
