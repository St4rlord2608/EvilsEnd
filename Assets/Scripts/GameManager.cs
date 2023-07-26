using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum SceneType
    {
        SafeHouse,
        MainMenu,
        GameScene
    }
    
    [SerializeField] private SceneType sceneType = SceneType.GameScene;

    private bool isPause = false;
    private bool isSafeHouse = false;
    private string currentScene;

    public event EventHandler OnGamePause;
    public event EventHandler OnGameResume;

    private void Awake()
    {
        Instance = this;
        currentScene = SceneManager.GetActiveScene().name;
        CurrentProgress.SetCurrentLevel(currentScene);
        if(sceneType == SceneType.SafeHouse)
        {
            isSafeHouse = true;
        }
        else
        {
            isSafeHouse = false;
        }
    }
    private void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;
    }

    private void Update()
    {
        
    }

    private void GameInput_OnPause(object sender, System.EventArgs e)
    {
        isPause = !isPause;

        HandleGamePause();
    }

    private void HandleGamePause()
    {
        if (isPause)
        {
            Time.timeScale = 0.0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);
            if (PauseMenuUI.Instance != null)
            {
                PauseMenuUI.Instance.Show();
            }
        }
        else
        {
            Time.timeScale = 1.0f;
            OnGameResume?.Invoke(this, EventArgs.Empty);
            if (PauseMenuUI.Instance != null)
            {
                PauseMenuUI.Instance.Hide();
            }
        }
    }

    public void Resume()
    {
        isPause = false;
        HandleGamePause();
    }

    public bool GetIsPause()
    {
        return isPause;
    }

    public SceneType GetSceneType()
    {
        return sceneType;
    }
}
