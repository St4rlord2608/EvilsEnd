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
    private static List<WorldQuest> worldQuestList = new List<WorldQuest>();

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
        foreach(KeyValuePair<string, LevelData> levelData in LevelDB.levelDataDictionary)
        {
            levelDataList.Add(levelData.Value);
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

    private static void CreateWorldQuestList()
    {
        worldQuestList = WorldQuestDB.GetQuests();
    }
    public static void SaveData()
    {
        CreatePlayerData();
        CreateCharacterList();
        CreateExploreLootList();
        CreateSafeHouseLootList();
        CreateLevelDataList();
        CreateWorldQuestList();
        CompleteData completeData = new CompleteData();
        completeData.SetPlayerData(playerData);
        completeData.SetProgressData(CurrentProgress.GetProgressData());
        completeData.SetCharacterData(characterList);
        completeData.SetExploreLootData(exploreLootList);
        completeData.SetSafeHouseLootData(safeHouseLootList);
        completeData.SetLevelData(levelDataList);
        completeData.SetWorldQuests(worldQuestList);
        SaveSystem.SaveData(completeData);
    }
}
