using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance;

    private const string UNAIMED_MOUSE_SENS = "unAimedMouseSens";
    private const string AIMED_MOUSE_SENS = "aimedMouseSens";

    [SerializeField] private Button increaseMusicButton;
    [SerializeField] private Button decreaseMusicButton;
    [Space]
    [SerializeField] private Button increaseSoundEffectsButton;
    [SerializeField] private Button decreaseSoundEffectsButton;
    [Space]
    [SerializeField] private Button increaseAimedMouseSensButton;
    [SerializeField] private Button decreaseAimedMouseSensButton;
    [Space]
    [SerializeField] private Button increaseUnAimedMouseSensButton;
    [SerializeField] private Button decreaseUnAimedMouseSensButton;
    [Space]
    [SerializeField] private Button backButton;
    [Space]
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI aimedMouseSensText;
    [SerializeField] private TextMeshProUGUI unAimedMouseSensText;

    private float aimedMouseSens = 1.0f;
    private float unAimedMouseSens = 1.0f;

    public event EventHandler<AimedMouseSensChangeEventArgs> onAimedMouseSensChange;
    public event EventHandler<UnAimedMouseSensChangeEventArgs> onUnAimedMouseSensChange;

    public class AimedMouseSensChangeEventArgs : EventArgs
    {
        public float aimedMouseSens;
    }

    public class UnAimedMouseSensChangeEventArgs : EventArgs
    {
        public float unAimedMouseSens;
    }

    private void Awake()
    {
        Instance = this;
        aimedMouseSens = PlayerPrefs.GetFloat(AIMED_MOUSE_SENS, 1f);
        unAimedMouseSens = PlayerPrefs.GetFloat(UNAIMED_MOUSE_SENS, 2f);
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
        increaseAimedMouseSensButton.onClick.AddListener(() =>
        {
            aimedMouseSens++;
            if(aimedMouseSens > 10)
            {
                aimedMouseSens = 10;
            }
            onAimedMouseSensChange?.Invoke(this, new AimedMouseSensChangeEventArgs
            {
                aimedMouseSens = this.aimedMouseSens
            });
            PlayerPrefs.SetFloat(AIMED_MOUSE_SENS, aimedMouseSens);
            PlayerPrefs.Save();
            UpdateVisual();
        });
        decreaseAimedMouseSensButton.onClick.AddListener(() =>
        {
            aimedMouseSens--;
            if(aimedMouseSens < 1)
            {
                aimedMouseSens = 1;
            }
            onAimedMouseSensChange?.Invoke(this, new AimedMouseSensChangeEventArgs
            {
                aimedMouseSens = this.aimedMouseSens
            });
            PlayerPrefs.SetFloat(AIMED_MOUSE_SENS, aimedMouseSens);
            PlayerPrefs.Save();
            UpdateVisual();
        });
        increaseUnAimedMouseSensButton.onClick.AddListener(() =>
        {
            unAimedMouseSens++;
            if(unAimedMouseSens > 10)
            {
                unAimedMouseSens = 10;
            }
            onUnAimedMouseSensChange?.Invoke(this, new UnAimedMouseSensChangeEventArgs
            {
                unAimedMouseSens = this.unAimedMouseSens
            });
            PlayerPrefs.SetFloat(UNAIMED_MOUSE_SENS, unAimedMouseSens);
            PlayerPrefs.Save();
            UpdateVisual();
        });
        decreaseUnAimedMouseSensButton.onClick.AddListener(() =>
        {
            unAimedMouseSens--;
            if(unAimedMouseSens < 1)
            {
                unAimedMouseSens = 1;
            }
            onUnAimedMouseSensChange?.Invoke(this, new UnAimedMouseSensChangeEventArgs
            {
                unAimedMouseSens = this.unAimedMouseSens
            });
            PlayerPrefs.SetFloat(UNAIMED_MOUSE_SENS, unAimedMouseSens);
            PlayerPrefs.Save();
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
        soundEffectText.text = "Sound effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
        aimedMouseSensText.text = "Aimed mouse sens: " + aimedMouseSens;
        unAimedMouseSensText.text = "Unaimed mouse sens: " + unAimedMouseSens;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public float GetAimSens()
    {
        return aimedMouseSens;
    }

    public float GetUnAimSens()
    {
        return unAimedMouseSens;
    }
}
