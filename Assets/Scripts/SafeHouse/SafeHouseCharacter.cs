using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouseCharacter : SafeHouseInteractable
{
    [SerializeField] private Transform visualsParent;
    [SerializeField] private Transform cameraFocusPoint;
    [SerializeField] private CharacterVisualsScriptableObject characterVisualsScriptableObject;
    [SerializeField] private CharacterStatsUI characterStatsUI;

    private Character character = new Character();
    private Transform selectedSpawnPoint;
    private Transform spawnPoint;
    private Transform focusedVirtualCameraTransform;
    private Transform meshTransform;
    private int visualIndex = 0;
    private int characterId;

    private bool isSelectedForNextMission = false;
    private bool debugActivateRandomVisual = false;

    private void Awake()
    {
    }

    private void Start()
    {
        if (debugActivateRandomVisual)
        {
            int visualIndex = Random.Range(0, visualsParent.childCount - 1);
            ActivateMeshTransform(visualIndex);
            character.id = characterId;
            character.visualIndex = visualIndex;
            character.sex = characterVisualsScriptableObject.characterVisuals[visualIndex].sex.ToString();
            if(character.sex == Character.FEMALE_SEX)
            {
                character.name = PossibleCharacterNames.femaleNames[Random.Range(0, PossibleCharacterNames.femaleNames.Count)];
            }
            else
            {
                character.name = PossibleCharacterNames.maleNames[Random.Range(0, PossibleCharacterNames.maleNames.Count)];
            }
            character.status = Character.ALIVE_STATUS;
            CharacterDB.AddCharacter(character);
        }
        else
        {
            ActivateMeshTransform(character.visualIndex);
        }
        if(characterStatsUI != null)
        {
            characterStatsUI.SetName(character.name);
        }
    }

    private void ActivateMeshTransform(int visualIndex)
    {
        this.visualIndex = visualIndex;
        string visualName = characterVisualsScriptableObject.characterVisuals[visualIndex].name;
        for (int i = 0; i < visualsParent.childCount; i++)
        {
            if (visualName == visualsParent.GetChild(i).name)
            {
                meshTransform = visualsParent.GetChild(i);
            }
        }
        if (meshTransform != null)
        {
            meshTransform.gameObject.SetActive(true);
            selectedTransform = meshTransform.GetChild(0);
        }
        else
        {
            Debug.LogError("no visual found");
        }
    }

    public void IsSelectedForNextMission(bool isSelectedForNextMission)
    {
        this.isSelectedForNextMission = isSelectedForNextMission;
        transform.parent = selectedSpawnPoint;
        transform.position = Vector3.zero;
    }

    public override void Interact(Transform player, bool slowFocusCameraPositioning = false)
    {
        focusedVirtualCameraTransform = safeHouseController.GetFocusCamera();
        Transform currentCameraFocusPoint = cameraFocusPoint;
        if (isSelectedForNextMission)
        {
            currentCameraFocusPoint = SafeHouseCharacterManager.Instance.GetSelectedCameraFocusPosition();
            SelectCharacterUI.Instance.Show();
        }else if(characterStatsUI != null)
        {
            characterStatsUI.Show();
        }
        focusedVirtualCameraTransform.gameObject.transform.position = currentCameraFocusPoint.position;
        focusedVirtualCameraTransform.gameObject.transform.rotation = currentCameraFocusPoint.rotation;
        focusedVirtualCameraTransform.gameObject.SetActive(true);
        safeHouseController.SetInteractableIsSelected(true);
    }

    public override void CancelCurrentAction()
    {
        if(focusedVirtualCameraTransform != null)
        {
            focusedVirtualCameraTransform.gameObject.SetActive(false);
            safeHouseController.SetInteractableIsSelected(false);
            SelectCharacterUI.Instance.Hide();
            if (characterStatsUI != null)
            {
                characterStatsUI.Hide();
            }
        }
    }

    public void SetSpawnPointTransform(Transform spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public void SelectThisCharacter(Transform selectedSpawnPoint)
    {
        this.selectedSpawnPoint = selectedSpawnPoint;
        isSelectedForNextMission = true;
        transform.parent = selectedSpawnPoint;
        transform.position = selectedSpawnPoint.position;
    }

    public void UnSelectThisCharacter()
    {
        isSelectedForNextMission = false;
        transform.parent = null;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    public int GetVisualIndex()
    {
        return visualIndex;
    }

    public void SetDebugSpawnRandomVisual(bool debug, int characterId)
    {
        this.debugActivateRandomVisual = debug;
        this.characterId = characterId;
    }

    public void SetCharacterInformation(Character character)
    {
        this.character = character;
    }

    public Character GetCharacter()
    {
        return this.character;
    }
    
}
