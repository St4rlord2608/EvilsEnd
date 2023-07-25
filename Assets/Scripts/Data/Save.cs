using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{

    private static PlayerData playerData;
    private static List<Character> characterList = new List<Character>();
    private static List<ExploreLoot> exploreLootList = new List<ExploreLoot>();
    private static List<SafeHouseLoot> safeHouseLootList = new List<SafeHouseLoot>();
    private static List<LevelData> levelDataList = new List<LevelData>();

    private static void CreatePlayerData()
    {
        playerData = new PlayerData()
        {
            health = 24,
            position = new float[3]
            {
                1, 0 , 3
            }

        };
    }

    private static void CreateLevelDataList()
    {
        levelDataList = new List<LevelData>();
        foreach (LevelData levelData in LevelDB.levelDataList)
        {
            levelDataList.Add(levelData);
        }
    }

    private static void CreateCharacterList()
    {
        characterList = new List<Character>();
        foreach (Character character in CharacterDB.allCharacters)
        {
            characterList.Add(character);
        }
    }

    private static void CreateExploreLootList()
    {
        exploreLootList = new List<ExploreLoot>();
        foreach (string type in LootTypeNames.names)
        {
            exploreLootList.Add(ExploreLootDB.GetExploreLoot(type));
        }
    }

    private static void CreateSafeHouseLootList()
    {
        safeHouseLootList = new List<SafeHouseLoot>();
        foreach (string type in LootTypeNames.names)
        {
            safeHouseLootList.Add(SafeHouseLootDB.GetSafeHouseLoot(type));
        }
    }
    public static void SaveData()
    {
        CreatePlayerData();
        CreateCharacterList();
        CreateExploreLootList();
        CreateSafeHouseLootList();
        CreateLevelDataList();
        CompleteData completeData = new CompleteData();
        completeData.SetPlayerData(playerData);
        completeData.SetProgressData(CurrentProgress.GetProgressData());
        completeData.SetCharacterData(characterList);
        completeData.SetExploreLootData(exploreLootList);
        completeData.SetSafeHouseLootData(safeHouseLootList);
        completeData.SetLevelData(levelDataList);
        SaveSystem.SaveData(completeData);
    }
}
