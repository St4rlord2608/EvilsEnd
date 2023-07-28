using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WorldQuest
{
    public string questName;
    public int questId;
    public WorldQuestTrigger[] onComplete;
    public WorldQuestTrigger[] onFailed;
    public WorldQuestTrigger[] onIsActive;
    public int[] idsOfCompletedNextQuests;
    public int[] idsOfFailedNextQuests;
    public bool isActive;
    public bool questCompleted;
    public bool questFailed;
}
