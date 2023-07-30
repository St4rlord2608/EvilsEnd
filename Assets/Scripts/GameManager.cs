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
    private bool isGameOver = false;
    private string currentScene;

    public event EventHandler OnGamePause;
    public event EventHandler OnGameResume;

    private void Awake()
    {
        ActivateMouseCursor();
        isGameOver = false;
        Resume();
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
        if(sceneType == SceneType.GameScene)
        {
            DisableMouseCursor();
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
        if(!isGameOver)
        {
            if (isPause)
            {
                
                ActivateMouseCursor();
                Time.timeScale = 0.0f;
                OnGamePause?.Invoke(this, EventArgs.Empty);
                if (PauseMenuUI.Instance != null)
                {
                    PauseMenuUI.Instance.Show();
                }
            }
            else
            {
                if (sceneType == SceneType.GameScene)
                {
                    DisableMouseCursor();
                }
                
                Time.timeScale = 1.0f;
                OnGameResume?.Invoke(this, EventArgs.Empty);
                if (PauseMenuUI.Instance != null)
                {
                    PauseMenuUI.Instance.Hide();
                }
            }
        }
        
    }

    private void DisableMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ActivateMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        isPause = false;
        HandleGamePause();
    }

    public void DeathPause()
    {
        isPause = true;
        isGameOver = true;
        if (isPause || isGameOver)
        {
            ActivateMouseCursor();
            Time.timeScale = 0.0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1.0f;
            OnGameResume?.Invoke(this, EventArgs.Empty);
        }
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
