using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterDB
{
    public static List<Character> allCharacters = new List<Character>();
    public static List<Character> aliveCharacters = new List<Character>();
    public static List<Character> deadCharacters = new List<Character>();

    public static bool hasBeenLoaded = false;
    public static void AddCharacter(Character character)
    {
        allCharacters.Insert(character.id, character);
        if(character.status == Character.ALIVE_STATUS)
        {
            aliveCharacters.Insert(character.id, character);
        }
        else
        {
            deadCharacters.Insert(character.id, character);
        }
        hasBeenLoaded = true;
    }

    public static Character GetCharacter(int id)
    {
        return allCharacters[id];
    }

    public static void MoveCharacterFromAliveToDead(int id)
    {
        aliveCharacters.RemoveAt(id);
        deadCharacters.Insert(id, allCharacters[id]);
    }

    public static void MoveCharacterFromDeadToAlive(int id)
    {
        deadCharacters.RemoveAt(id);
        aliveCharacters.Insert(id, allCharacters[id]);
    }

    public static Character GetPlayerCharacter()
    {
        foreach(Character character in aliveCharacters)
        {
            if(character.isPlayer) return character;
        }
        return aliveCharacters[0];
    }

    public static void ClearCharacterDB()
    {
        allCharacters.Clear();
        aliveCharacters.Clear();
        deadCharacters.Clear();
    }

}
