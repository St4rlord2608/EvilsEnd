using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsUI : MonoBehaviour
{
    public static MainMenuSettingsUI Instance;

    [SerializeField] private Button increaseMusicButton;
    [SerializeField] private Button decreaseMusicButton;
    [SerializeField] private Button increaseSoundEffectsButton;
    [SerializeField] private Button decreaseSoundEffectsButton;
    [SerializeField] private Button backButton;
    [Space]
    [SerializeField] private Transform settingsVirtualCamTransform;
    [Space]
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundEffectText;

    private void Awake()
    {
        Instance = this;
        Hide();
        increaseSoundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.IncreaseVolume();
            UpdateVisual();
        });
        decreaseSoundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.DecreaseVolume();
            UpdateVisual();
        });
        increaseMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.IncreaseVolume();
            UpdateVisual();
        });
        decreaseMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.DecreaseVolume();
            UpdateVisual();
        });
        backButton.onClick.AddListener(() =>
        {
            settingsVirtualCamTransform.gameObject.SetActive(false);
            MainMenuUI.Instance.Show();
            Hide();
        });
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
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
