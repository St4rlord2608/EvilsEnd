using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadUI : MonoBehaviour
{
    public static LoadUI Instance;

    [SerializeField] private Transform loadVirtualCamTransform;
    [SerializeField] private Button save_1_Button;
    [SerializeField] private Button save_2_Button;
    [SerializeField] private Button save_3_Button;
    [SerializeField] private Button backButton;

    private List<Button> buttonList = new List<Button>();

    private void Awake()
    {
        buttonList.Add(save_1_Button);
        buttonList.Add(save_2_Button);
        buttonList.Add(save_3_Button);
        Instance = this;
        int index = 0;
        foreach(Button button in buttonList)
        {
            index++;
            ProgressData progressData = SaveSystem.GetProgressDataForSaveGame("/save_" + index);
            if (progressData.saveName == null)
            {
                button.gameObject.SetActive(false);
            }
        }
        Hide();
        save_1_Button.onClick.AddListener(() =>
        {
            LoadSaveGame("/save_1");
        });
        save_2_Button.onClick.AddListener(() =>
        {
            LoadSaveGame("/save_2");
        });
        save_3_Button.onClick.AddListener(() =>
        {
            LoadSaveGame("/save_3");
        });
        backButton.onClick.AddListener(() =>
        {
            loadVirtualCamTransform.gameObject.SetActive(false);
            MainMenuUI.Instance.Show();
            Hide();
        });
        SaveSystem.OnNewGameCreated += SaveSystem_OnNewGameCreated;
    }

    private void LoadSaveGame(string saveName)
    {
        SaveSystem.SetSaveGameFolderName(saveName);
        Load.LoadAllData();
        LoadScene();
    }

    private void LoadScene()
    {
        if(CurrentProgress.GetProgressData().isInSaveHouse)
        {
            SceneLoader.Load(SceneLoader.Scene.SafeHouse);
        }
        else
        {
            SceneManager.LoadScene(LevelDB.GetLevel(CurrentProgress.GetProgressData().currentLevelID).levelName);
        }
        
    }

    private void SaveSystem_OnNewGameCreated(object sender, EventArgs e)
    {
        TryToLoadSaveGames();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void TryToLoadSaveGames()
    {
        int index = 0;
        foreach (Button button in buttonList)
        {
            index++;
            ProgressData progressData = SaveSystem.GetProgressDataForSaveGame("/save_" + index);
            if (progressData.saveName == null)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
            }
        }
    }
}
