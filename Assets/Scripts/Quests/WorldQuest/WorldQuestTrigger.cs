using System.Collections;
using System.Collections.Generic;
using Unity.IO.Archive;
using UnityEngine;

[System.Serializable]
public class WorldQuestTrigger
{
    public enum TriggerType
    {
        ChangeLevelActiveState,
        somethingDifferent
    }

    [SerializeField] private TriggerType type;
    [Space]
    [SerializeField] private bool levelIsActive;
    [SerializeField] public LoadLevelUI loadLevelUI;

    public void Trigger()
    {
        switch (type)
        {
            case TriggerType.ChangeLevelActiveState:
                ChangeLevelActiveState();
                break;
        }
    }
    private void ChangeLevelActiveState()
    {
        
        string levelName = loadLevelUI.GetLevelName();
        Debug.Log(loadLevelUI);
        if (LevelDB.CheckIfLevelIsInDictionary(levelName))
        {
            LevelData levelData = LevelDB.GetLevel(levelName);
            if (levelIsActive)
            {
                levelData.unlocked = true;
            }
            else
            {
                levelData.unlocked = false;
            }
            LevelDB.UpdateLevel(levelData);
            loadLevelUI.gameObject.SetActive(levelData.unlocked);

        }
        else
        {
            loadLevelUI.gameObject.SetActive(false);
        }
    }
}
