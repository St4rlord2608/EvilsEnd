using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct Quest
{
    public enum QuestType
    {
        Interact,
        Kill,
        ClearArea,
        GoToPosition
    }

    public string QuestName;
    public QuestType Type;
    public int enemiesNeededToKill;
    public Transform[] QuestTransform;
    public QuestLock[] QuestLocks;
}
