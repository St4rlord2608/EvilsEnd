using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Transform visualsParent;
    [SerializeField] private CharacterVisualsScriptableObject characterVisualsScriptableObject;

    private Transform activeVisual;
    private Character character;

    private void Start()
    {
        character = CharacterDB.GetPlayerCharacter();
        string visualName = characterVisualsScriptableObject.characterVisuals[character.visualIndex].name;
        for(int i = 0; i <  visualsParent.childCount; i++)
        {
            if(visualName == visualsParent.GetChild(i).name)
            {
                activeVisual = visualsParent.GetChild(i);
            }
        }
        if(activeVisual != null)
        {
            activeVisual.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("no visual found");
        }
    }
}
