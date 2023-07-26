using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelProgressionManager : MonoBehaviour
{
    [SerializeField] private LevelData[] allLevelData;

    private void Awake()
    {
        LevelDB.ClearLevelDataDictionary();
        foreach (LevelData levelData in allLevelData)
        {
            LevelDB.AddLevel(levelData);
        }
    }
}
