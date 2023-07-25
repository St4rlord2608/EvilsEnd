using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelDB
{
    public static List<LevelData> levelDataList = new List<LevelData>();

    public static void ClearLevelDataList()
    {
        levelDataList.Clear();
    }
    
    public static void AddLevel(LevelData levelData)
    {
        levelData.levelID = levelDataList.Count;
        levelDataList.Add(levelData);
    }

    public static LevelData GetLevel(int levelID)
    {
        return levelDataList[levelID];
    }
}
