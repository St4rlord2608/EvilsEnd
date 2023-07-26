using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelDB
{
    public static Dictionary<string,LevelData> levelDataDictionary = new Dictionary<string, LevelData>();

    public static void ClearLevelDataDictionary()
    {
        levelDataDictionary.Clear();
    }
    
    public static void AddLevel(LevelData levelData)
    {
        levelDataDictionary.Add(levelData.levelName, levelData);
    }

    public static LevelData GetLevel(string levelName)
    {
        return levelDataDictionary[levelName];
    }

    public static bool CheckIfLevelIsInDictionary(string levelname)
    {
        if(levelDataDictionary.ContainsKey(levelname))
        {
            return true;    
        }
        return false;
    }
}
