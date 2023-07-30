using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenUI : MonoBehaviour
{
    public static DeathScreenUI Instance;

    [SerializeField] private Button restartLevelButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Instance = this;
        restartLevelButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadFromString(SceneManager.GetActiveScene().name);
        });
        exitButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.MainMenu);
        });
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
