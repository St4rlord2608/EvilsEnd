using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameUI : MonoBehaviour
{
    public static NewGameUI Instance;

    [SerializeField] private Transform newVirtualCamTransform;
    [SerializeField] private Button save_1_Button;
    [SerializeField] private Button save_2_Button;
    [SerializeField] private Button save_3_Button;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        Instance = this;
        Hide();
        save_1_Button.onClick.AddListener(() =>
        {
            CreateNewGame("/save_1");
        });
        save_2_Button.onClick.AddListener(() =>
        {
            CreateNewGame("/save_2");
        });
        save_3_Button.onClick.AddListener(() =>
        {
            CreateNewGame("/save_3");
        });
        backButton.onClick.AddListener(() =>
        {
            newVirtualCamTransform.gameObject.SetActive(false);
            MainMenuUI.Instance.Show();
            Hide();
        });
    }

    private void CreateNewGame(string saveName)
    {
        ProgressData progressData = new ProgressData()
        {
            saveName = saveName,
            currentLevelName = "IntroLevel"
        };
        CurrentProgress.SetProgressData(progressData);
        SaveSystem.CreateNewGame(progressData.saveName);
        LoadSaveGame(saveName);
    }

    private void LoadSaveGame(string saveName)
    {
        SaveSystem.SetSaveGameFolderName(saveName);
        Load.LoadAllData();
        LoadScene();
    }

    private void LoadScene()
    {
        SceneLoader.LoadFromString(CurrentProgress.GetProgressData().currentLevelName);
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
