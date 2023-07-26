using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Load
{
    public static bool HasBeenLoaded = false;
    public static void LoadAllData()
    {
        ExploreLootDB.Init();
        SafeHouseLootDB.Init();
        CompleteData completeData = SaveSystem.LoadData();
        if (completeData != null)
        {
            CurrentProgress.SetProgressData(completeData.ProgressData);
            WriteCharacterDB(completeData);
            WriteExploreLootDB(completeData);
            WriteSafeHouseLootDB(completeData);
            WriteLevelDB(completeData);
        }
        HasBeenLoaded = true;
    }

    private static void WriteCharacterDB(CompleteData completeData)
    {
        CharacterDB.ClearCharacterDB();
        foreach (Character character in completeData.CharacterData)
        {
            CharacterDB.AddCharacter(character);
        }
    }

    private static void WriteExploreLootDB(CompleteData completeData)
    {
        ExploreLootDB.ClearExploreLoot();
        foreach (ExploreLoot loot in completeData.ExploreLootData)
        {
            ExploreLootDB.UpdateValueOfExploreLoot(loot);
        }
    }

    private static void WriteSafeHouseLootDB(CompleteData completeData)
    {
        SafeHouseLootDB.ClearSafeHouseLoot();
        foreach (SafeHouseLoot loot in completeData.SafeHouseLootData)
        {
            SafeHouseLootDB.UpdateValueOfSafeHouseLoot(loot);
        }
    }

    private static void WriteLevelDB(CompleteData completeData)
    {
        LevelDB.ClearLevelDataDictionary();
        foreach(LevelData levelData in completeData.LevelDatas)
        {
            LevelDB.AddLevel(levelData);
        }
    }
}
