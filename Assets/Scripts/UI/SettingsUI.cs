using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance;

    [SerializeField] private Button increaseMusicButton;
    [SerializeField] private Button decreaseMusicButton;
    [SerializeField] private Button increaseSoundEffectsButton;
    [SerializeField] private Button decreaseSoundEffectsButton;
    [SerializeField] private Button backButton;
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
            PauseMenuUI.Instance.Show();
            Hide();
        });

    }

    private void GameManager_OnGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Start()
    {
        GameManager.Instance.OnGameResume += GameManager_OnGameResume;
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
