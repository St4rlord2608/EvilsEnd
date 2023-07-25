using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance;

    public event EventHandler OnResumeGame;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    private void Awake()
    {
        Instance = this;
        Hide();

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Resume();
        });
        settingsButton.onClick.AddListener(() =>
        {
            SettingsUI.Instance.Show();
            Hide();
        });
        exitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Resume();
            SceneLoader.Load(SceneLoader.Scene.MainMenu);
        });
        
    }

    private void Start()
    {
        GameManager.Instance.OnGameResume += GameManager_OnGameResume;
    }

    private void GameManager_OnGameResume(object sender, System.EventArgs e)
    {
        Hide();
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
