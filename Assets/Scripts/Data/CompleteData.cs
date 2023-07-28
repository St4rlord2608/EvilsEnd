using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteData
{
    public PlayerData PlayerData;
    public ProgressData ProgressData;
    public List<Character> CharacterData = new List<Character>();
    public List<ExploreLoot> ExploreLootData = new List<ExploreLoot>();
    public List<SafeHouseLoot> SafeHouseLootData = new List<SafeHouseLoot>();
    public List<LevelData> LevelDatas = new List<LevelData>();
    public List<WorldQuest> WorldQuests = new List<WorldQuest>();


    public void SetPlayerData(PlayerData playerData)
    {
        PlayerData = playerData;
    }

    public void SetProgressData(ProgressData progressData)
    {
        ProgressData = progressData;
    }

    public void SetCharacterData(List<Character> characterData)
    {
        CharacterData = characterData;
    }

    public void SetExploreLootData(List<ExploreLoot> exploreLootData)
    {
        ExploreLootData  = exploreLootData;
    }

    public void SetSafeHouseLootData(List<SafeHouseLoot> safeHouseLootData)
    {
        SafeHouseLootData = safeHouseLootData;
    }

    public void AddCharacterData(Character character)
    {
        CharacterData.Add(character);
    }

    public void SetLevelData(List<LevelData> levelDatas)
    {
        LevelDatas = levelDatas;
    }

    public void SetWorldQuests(List<WorldQuest> worldQuests)
    {
        WorldQuests = worldQuests;
    }
}
