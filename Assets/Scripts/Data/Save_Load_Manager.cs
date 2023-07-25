using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string debugSaveGameName = "";


    private void Awake()
    {
        Instance = this;
        if(debugSaveGameName != "")
        {
            SaveSystem.SetSaveGameFolderName(debugSaveGameName);
        }
        if (!Load.HasBeenLoaded)
        {
            Load.LoadAllData();
        }
        
    }

    

    private void OnDestroy()
    {
        if(SceneManager.GetActiveScene().name == SceneLoader.Scene.SafeHouse.ToString())
        {
            Save.SaveData();
        }
    }
}
