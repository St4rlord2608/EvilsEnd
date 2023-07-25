using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SafeHouseCharacterManager : MonoBehaviour
{
    public static SafeHouseCharacterManager Instance;

    [SerializeField] private int livingCharacterCount;
    [SerializeField] private Transform baseCharacterPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform selectedSpawnPoint;
    [SerializeField] private Transform cameraFocusPosition;
    [SerializeField] private bool DebugSpawnRandomSurvivors;

    private int currentSelectedCharacterIndex = 0;
    private SafeHouseCharacter currenSelectedCharacter;
    private List<Transform> aliveCharacters = new List<Transform>();

    private void Awake()
    {
        Instance = this;
        if (CharacterDB.hasBeenLoaded)
        {
            DebugSpawnRandomSurvivors = false;

        }
        if (DebugSpawnRandomSurvivors)
        {
            SpawnRandomSurvivors();
        }
        else
        {
            LoadCharactersFromDB();
        }
    }

    private void SpawnRandomSurvivors()
    {
        for (int survivorCount = 0; survivorCount < livingCharacterCount; survivorCount++)
        {
            Transform survivor;
            survivor = Instantiate(baseCharacterPrefab, spawnPoints[survivorCount].position,
                       spawnPoints[survivorCount].rotation);
            SafeHouseCharacter safeHouseCharacter = survivor.GetComponent<SafeHouseCharacter>();
            safeHouseCharacter.SetDebugSpawnRandomVisual(true, survivorCount);
            safeHouseCharacter.SetSpawnPointTransform(spawnPoints[survivorCount]);
            if (survivorCount == 0)
            {
                safeHouseCharacter.SelectThisCharacter(selectedSpawnPoint);
                currenSelectedCharacter = safeHouseCharacter;
            }
            aliveCharacters.Add(survivor);
        }
    }

    private void LoadCharactersFromDB()
    {
        int assignedSpawnPoints = 0;
        for (int characterId = 0; characterId < CharacterDB.allCharacters.Count; characterId++)
        {
            Character character = new Character();
            character = CharacterDB.GetCharacter(characterId);
            if (character.status == Character.ALIVE_STATUS)
            {
                SpawnLoadedSurvivors(character, assignedSpawnPoints);
                assignedSpawnPoints++;
            }
        }
    }

    private void SpawnLoadedSurvivors(Character character, int spawnPointIndex)
    {
        Transform survivor;
        survivor = Instantiate(baseCharacterPrefab, spawnPoints[spawnPointIndex].position,
                   spawnPoints[spawnPointIndex].rotation);
        SafeHouseCharacter safeHouseCharacter = survivor.GetComponent<SafeHouseCharacter>();
        safeHouseCharacter.SetCharacterInformation(character);
        safeHouseCharacter.SetSpawnPointTransform(spawnPoints[spawnPointIndex]);
        if (spawnPointIndex == 0)
        {
            safeHouseCharacter.SelectThisCharacter(selectedSpawnPoint);
            currenSelectedCharacter = safeHouseCharacter;
        }
        aliveCharacters.Add(survivor);
    }

    public SafeHouseCharacter GetSelectedCharacter()
    {
        return currenSelectedCharacter;
    }

    public Transform GetSelectedSpawnPoint()
    {
        return selectedSpawnPoint;
    }

    public Transform GetSelectedCameraFocusPosition()
    {
        return cameraFocusPosition;
    }

    public void SelectNextCharacter()
    {
        currenSelectedCharacter.UnSelectThisCharacter();
        currentSelectedCharacterIndex++;
        if(currentSelectedCharacterIndex >= aliveCharacters.Count)
        {
            currentSelectedCharacterIndex = 0;
        }
        Transform nextCharacter = aliveCharacters[currentSelectedCharacterIndex];
        SafeHouseCharacter safeHouseCharacter = nextCharacter.GetComponent<SafeHouseCharacter>();
        safeHouseCharacter.SelectThisCharacter(selectedSpawnPoint);
        currenSelectedCharacter = safeHouseCharacter;
    }

    public void SelectPreviousCharacter()
    {
        currenSelectedCharacter.UnSelectThisCharacter();
        currentSelectedCharacterIndex--;
        if(currentSelectedCharacterIndex < 0)
        {
            currentSelectedCharacterIndex = aliveCharacters.Count - 1;
        }
        Transform nextCharacter = aliveCharacters[currentSelectedCharacterIndex];
        SafeHouseCharacter safeHouseCharacter = nextCharacter.GetComponent<SafeHouseCharacter>();
        safeHouseCharacter.SelectThisCharacter(selectedSpawnPoint);
        currenSelectedCharacter = safeHouseCharacter;
    }
}
