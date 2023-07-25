using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedPlayerInfo : MonoBehaviour
{
    
    [SerializeField] private string CharacterName;
    [SerializeField] private Transform characterVisual;
    [SerializeField] private string sex;
    [SerializeField] private CharacterVisualsScriptableObject characterVisualData;

    private int id = 0;
    private Character character;

    private void Awake()
    {
        if(CharacterDB.allCharacters.Count != 0)
        {
            id = CharacterDB.allCharacters.Count;
        }
        string visualName = characterVisual.name;
        int visualIndex = 0;
        characterVisual.gameObject.SetActive(true);
        for (int i = 0; i < characterVisualData.characterVisuals.Length; i++)
        {
            if (visualName == characterVisualData.characterVisuals[i].name)
            {
                visualIndex = i;
            }
        }
        character = new Character
        {
            id = this.id,
            name = this.CharacterName,
            visualIndex = visualIndex,
            status = Character.ALIVE_STATUS,
            sex = this.sex,
            isPlayer = true
        };
    }

    public Character GetScriptedCharacter()
    {
        return character;
    }
}
