using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldQuestManager : MonoBehaviour
{
    [SerializeField] private List<WorldQuest> worldQuestList;
    [SerializeField] private bool DebugOverrideWorldQuests;

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
        if (!CurrentProgress.GetProgressData().WorldQuestsHaveBeenInitialized || DebugOverrideWorldQuests)
        {
            WorldQuestDB.SetQuests(worldQuestList);
            ProgressData progressData = CurrentProgress.GetProgressData();
            progressData.WorldQuestsHaveBeenInitialized = true;
            CurrentProgress.SetProgressData(progressData);
        }
        else
        {
            List<WorldQuest> dbWorldQuestList = WorldQuestDB.GetQuests();
            for (int index = 0; index < dbWorldQuestList.Count; index++)
            {
                WorldQuest worldQuest = worldQuestList[index];
                worldQuest.isActive = dbWorldQuestList[index].isActive;
                Debug.Log(dbWorldQuestList[index].isActive);
                worldQuest.questCompleted = dbWorldQuestList[index].questCompleted;
                worldQuest.questFailed = dbWorldQuestList[index].questFailed;
                worldQuestList[index] = worldQuest;
            }
        }
        foreach(WorldQuest worldQuest in worldQuestList)
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
            WorldQuest worldQuest = worldQuestList[id];
            worldQuest.isActive = false;
            worldQuest.questCompleted = true;
            HandleIsCompleted(worldQuest);
            if(worldQuest.idsOfCompletedNextQuests.Length > 0)
            {
                foreach(int nextQuestId in worldQuest.idsOfCompletedNextQuests)
                {
                    WorldQuest nextWorldQuest = worldQuestList[nextQuestId];
                    nextWorldQuest.isActive = true;
                    HandleIsActive(nextWorldQuest);
                    worldQuestList[nextQuestId] = nextWorldQuest;
                }
            }
            worldQuestList[id] =  worldQuest;
        }
        WorldQuestDB.SetQuests(worldQuestList);
        WorldQuestHandler.ClearCompletedQuestIds();
    }
    private void HandleFailedQuests()
    {
        foreach (int id in WorldQuestHandler.GetAllFailedQuestIds())
        {
            WorldQuest worldQuest = worldQuestList[id];
            worldQuest.isActive = false;
            worldQuest.questFailed = true;
            HandleIsFailed(worldQuest);
            if (worldQuest.idsOfFailedNextQuests.Length > 0)
            {
                foreach (int nextQuestId in worldQuest.idsOfFailedNextQuests)
                {
                    WorldQuest nextWorldQuest = worldQuestList[nextQuestId];
                    nextWorldQuest.isActive = true;
                    HandleIsActive(nextWorldQuest);
                    worldQuestList[nextQuestId] = nextWorldQuest;
                }
            }
            worldQuestList[id] = worldQuest;
        }
        WorldQuestDB.SetQuests(worldQuestList);
        WorldQuestHandler.ClearFailedQuestIds();
    }

    private void HandleIsActive(WorldQuest worldQuest)
    {
        ActiveQuestsUI.Instance.SetActiveQuest(worldQuest);
        ActiveQuestsUI.Instance.Show();
        foreach (WorldQuestTrigger worldQuestTrigger in worldQuest.onIsActive)
        {
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
        WorldQuestDB.SetQuests(worldQuestList);
        WorldQuestHandler.OnQuestCompleted -= WorldQuestHandler_OnQuestCompleted;
        WorldQuestHandler.OnQuestFailed -= WorldQuestHandler_OnQuestFailed;
    }
}
