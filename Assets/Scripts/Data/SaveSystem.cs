using UnityEngine;
using JetBrains.Annotations;
using System.IO;
using System;

public static class SaveSystem
{
    private static string saveGameFolderName;

    public static event EventHandler OnNewGameCreated;
    public static void SaveData(CompleteData completeData)
    {
        string completeDataJSON = JsonUtility.ToJson(completeData);
        File.WriteAllText(Application.persistentDataPath + saveGameFolderName + "/save.txt", completeDataJSON);
    }

    public static CompleteData LoadData()
    {
        if(File.Exists(Application.persistentDataPath + saveGameFolderName + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + saveGameFolderName + "/save.txt");

            CompleteData completeData = JsonUtility.FromJson<CompleteData>(saveString);

            return completeData;
        }
        else
        {
            Debug.Log("file not found in " + Application.persistentDataPath + saveGameFolderName);
            return null;
        }
    }

    public static void CreateNewGame(string folderName)
    {
        saveGameFolderName = folderName;
        if (!Directory.Exists(Application.persistentDataPath + folderName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + folderName);
        }
        else
        {
            if(File.Exists(Application.persistentDataPath + folderName + "/save.txt"))
            {
                File.Delete(Application.persistentDataPath + saveGameFolderName + "/save.txt");
            }
            Directory.Delete(Application.persistentDataPath + saveGameFolderName);
            Directory.CreateDirectory(Application.persistentDataPath + saveGameFolderName);
        }
        SaveData(new CompleteData()
        {
            ProgressData = new ProgressData()
            {
                saveName = folderName
            },
            LevelDatas = LevelDB.levelDataList
            
        });
        OnNewGameCreated?.Invoke(null, EventArgs.Empty);
    }

    public static ProgressData GetProgressDataForSaveGame(string folderName)
    {
        saveGameFolderName = folderName;
        if(!Directory.Exists(Application.persistentDataPath + folderName))
        {
            return new ProgressData();
        }
        else if(File.Exists(Application.persistentDataPath + folderName + "/save.txt"))
        {
            CompleteData completeData = LoadData();
            return completeData.ProgressData;
        }
        return new ProgressData();
    }

    public static string GetSaveGameFolderName()
    {
        return saveGameFolderName;
    }

    public  static void SetSaveGameFolderName(string saveGameFolderName)
    {
        SaveSystem.saveGameFolderName = saveGameFolderName;
    }
}
