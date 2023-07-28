using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldQuestManager : MonoBehaviour
{
    [SerializeField] private List<WorldQuest> worldQuestsList;

    private void Awake()
    {
        WorldQuestHandler.OnQuestCompleted += WorldQuestHandler_OnQuestCompleted;
        WorldQuestHandler.OnQuestFailed += WorldQuestHandler_OnQuestFailed;
    }

    private void WorldQuestHandler_OnQuestFailed(object sender, System.EventArgs e)
    {
        HandleFailedQuests();
    }

    private void WorldQuestHandler_OnQuestCompleted(object sender, System.EventArgs e)
    {
        HandleCompletedQuests();
    }

    private void Start()
    {
        if (!CurrentProgress.GetProgressData().WorldQuestsHaveBeenInitialized)
        {
            Debug.Log("set quests");
            WorldQuestDB.SetQuests(worldQuestsList);
            ProgressData progressData = CurrentProgress.GetProgressData();
            progressData.WorldQuestsHaveBeenInitialized = true;
            CurrentProgress.SetProgressData(progressData);
        }
        else
        {
            worldQuestsList = WorldQuestDB.GetQuests();
        }
        foreach(WorldQuest worldQuest in worldQuestsList)
        {
            if(worldQuest.isActive)
            {
                HandleIsActive(worldQuest);
            }
        }
        HandleCompletedQuests();
        HandleFailedQuests();
    }

    private void HandleCompletedQuests()
    {
        foreach(int id in WorldQuestHandler.GetAllCompletedQuestIds()) {
            WorldQuest worldQuest = worldQuestsList[id];
            worldQuest.isActive = false;
            worldQuest.questCompleted = true;
            HandleIsCompleted(worldQuest);
            if(worldQuest.idsOfCompletedNextQuests.Length > 0)
            {
                foreach(int nextQuestId in worldQuest.idsOfCompletedNextQuests)
                {
                    WorldQuest nextWorldQuest = worldQuestsList[nextQuestId];
                    nextWorldQuest.isActive = true;
                    HandleIsActive(nextWorldQuest);
                }
            }
        }
        WorldQuestHandler.ClearCompletedQuestIds();
    }
    private void HandleFailedQuests()
    {
        foreach (int id in WorldQuestHandler.GetAllFailedQuestIds())
        {
            WorldQuest worldQuest = worldQuestsList[id];
            worldQuest.isActive = false;
            worldQuest.questFailed = true;
            HandleIsFailed(worldQuest);
            if (worldQuest.idsOfFailedNextQuests.Length > 0)
            {
                foreach (int nextQuestId in worldQuest.idsOfFailedNextQuests)
                {
                    WorldQuest nextWorldQuest = worldQuestsList[nextQuestId];
                    nextWorldQuest.isActive = true;
                    HandleIsActive(nextWorldQuest);
                }
            }
        }
        WorldQuestHandler.ClearFailedQuestIds();
    }

    private void HandleIsActive(WorldQuest worldQuest)
    {
        ActiveQuestsUI.Instance.SetActiveQuest(worldQuest);
        ActiveQuestsUI.Instance.Show();
        foreach (WorldQuestTrigger worldQuestTrigger in worldQuest.onIsActive)
        {
            Debug.Log(worldQuestTrigger.loadLevelUI);
            worldQuestTrigger.Trigger();
        }
    }

    private void HandleIsCompleted(WorldQuest worldQuest)
    {
        foreach(WorldQuestTrigger worldQuestTrigger in worldQuest.onComplete)
        {
            worldQuestTrigger.Trigger();
        }
    }

    private void HandleIsFailed(WorldQuest worldQuest)
    {
        foreach (WorldQuestTrigger worldQuestTrigger in worldQuest.onFailed)
        {
            worldQuestTrigger.Trigger();
        }
    }

    private void OnDestroy()
    {
        WorldQuestDB.SetQuests(worldQuestsList);
        WorldQuestHandler.OnQuestCompleted -= WorldQuestHandler_OnQuestCompleted;
        WorldQuestHandler.OnQuestFailed -= WorldQuestHandler_OnQuestFailed;
    }
}
