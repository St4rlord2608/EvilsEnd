using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class VehicleInteractable : Interactable
{
    private PlayerInventory inventory;
    public override void Interact(Transform player)
    {
        if(player.TryGetComponent<ScriptedPlayerInfo>(out ScriptedPlayerInfo scriptedPlayerInfo))
        {
            Character character = scriptedPlayerInfo.GetScriptedCharacter();
            if(CharacterDB.allCharacters.Count == 0)
            {
                //character.id = CharacterDB.allCharacters.Count;
            }
            else
            {
                //character.id = CharacterDB.allCharacters.Count - 1;
            }
            
            CharacterDB.AddCharacter(character);
        }
        if(player.TryGetComponent<PlayerWeaponHandler>(out PlayerWeaponHandler playerWeaponHandler))
        {
            int ammunitionInCurrentMagazine = playerWeaponHandler.GetAmmunitionInCurrentMagazine();
            if(player.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
            {
                playerInventory.AddToInventory(playerWeaponHandler.GetCurrentWeaponType(), ammunitionInCurrentMagazine);
            }
        }
        Save.SaveData();
        SceneLoader.Load(SceneLoader.Scene.SafeHouse);
    }
}
