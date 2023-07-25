using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSafeHouseInteractable : SafeHouseInteractable
{
    private void Awake()
    {
        isExit = true;
    }
    public override void SetIsSelected(Transform player)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        if(safeHouseController.GetIsInEntrance())
        {
            InteractActionUI.Instance.SetTextAndActivate(GetSelectedDisplayHint());
            selectedTransform.gameObject.SetActive(true);
        }
    }
    public override void SetIsUnselected(Transform player)
    {
        InteractActionUI.Instance.Hide();
        selectedTransform.gameObject.SetActive(false);
    }
    public override void CancelCurrentAction()
    {
        
    }

    public override void Interact(Transform player, bool slowFocusCameraPositioning = false)
    {
        safeHouseController = player.GetComponent<SafeHouseController>();
        if(safeHouseController.GetIsInEntrance())
        {
            SafeHouseCharacter safeHouseCharacter = SafeHouseCharacterManager.Instance.GetSelectedCharacter();
            Destroy(SafeHouseInventoryManager.Instance);
            ProgressData progressData = CurrentProgress.GetProgressData();
            SceneLoader.LoadFromString(LevelDB.GetLevel(progressData.currentLevelID).levelName);
        }
    }
}
