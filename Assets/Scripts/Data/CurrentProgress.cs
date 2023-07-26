using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentProgress
{
    private static ProgressData progressData;

    public static void SetProgressData(ProgressData progressData)
    {
        CurrentProgress.progressData = progressData;
    }

    public static ProgressData GetProgressData()
    {
        return progressData;
    }

    public static void SetCurrentLevel(string levelName)
    {
        progressData.currentLevelName = levelName;
    }

    //public static void CompletetLevel()
    //{
    //    progressData.currentLevelID++;
    //}

    //public static void SetIsSafeHouse(bool isInSaveHouse)
    //{
    //    progressData.isInSaveHouse = isInSaveHouse;
    //}
}
