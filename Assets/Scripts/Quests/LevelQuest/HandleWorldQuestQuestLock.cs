using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWorldQuestQuestLock : QuestLock
{
    [SerializeField] private bool questFailed;
    [SerializeField] private int worldQuestId;
    public override void QuestCompleted()
    {
        if (questFailed)
        {
            WorldQuestHandler.FailedQuest(worldQuestId);
        }
        else
        {
            WorldQuestHandler.CompletedQuest(worldQuestId);
        }
    }
}
