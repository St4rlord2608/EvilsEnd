using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public static QuestHandler Instance;

    [SerializeField] private Quest[] questList;
    [SerializeField] private ParticleSystem questPositionArrowEffectPrefab;

    private int questIndex = 0;
    private int taskInCurrentQuestCount = 0;
    private int completedTaskAmountInCurrentQuest = 0;
    private int killedEnemiesCount = 0;
    private List<ParticleSystem> currentQuestPositionMarkers = new List<ParticleSystem>();

    private bool allQuestCompleted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (questList[questIndex].QuestTransform.Length > 0)
        {
            if(QuestDirectionUI.Instance != null)
            {
                QuestDirectionUI.Instance.SetQuestTransform(questList[questIndex].QuestTransform[0]);
            }
            
        }
        
    }

    private void Update()
    {

    }

    //private void HandleQuestPositionMarkers()
    //{
        
    //    if (GetCurrentQuest().QuestTransform != null)
    //    {
    //        ClearQuestPositionMarkers();
    //        foreach (Transform questTransform in GetCurrentQuest().QuestTransform)
    //        {
    //            ParticleSystem questPositionArrow = Instantiate(questPositionArrowEffectPrefab, questTransform.position + Vector3.up * 6,
    //                Quaternion.identity);
    //            currentQuestPositionMarkers.Add(questPositionArrow);
    //        }

    //    }
    //}

    //private void ClearQuestPositionMarkers()
    //{
    //    foreach (ParticleSystem questPositionMarker in currentQuestPositionMarkers)
    //    {
    //        Destroy(questPositionMarker.gameObject);
    //    }
    //    currentQuestPositionMarkers.Clear();
    //}

    private void CurrentQuestCompleted()
    {
        HandleQuestLock();
        completedTaskAmountInCurrentQuest = 0;
        if(questIndex >= questList.Length -1)
        {
            if (QuestUI.Instance != null)
            {
                allQuestCompleted = true;
            }
            return;
        }
        questIndex++;
        if(QuestUI.Instance != null )
        {
            if (GetCurrentQuest().Type == Quest.QuestType.Kill)
            {
                QuestUI.Instance.SetTitle(GetCurrentQuest().QuestName + "( " + killedEnemiesCount + " )");
            }
            else if (GetCurrentQuest().Type == Quest.QuestType.ClearArea)
            {
                int leftEnemiesAmount = GetCurrentQuest().enemiesNeededToKill - killedEnemiesCount;
                QuestUI.Instance.SetTitle(GetCurrentQuest().QuestName + "( " + leftEnemiesAmount + " )");
            }
            else
            {
                QuestUI.Instance.SetTitle(GetCurrentQuest().QuestName);
            }
            if (questList[questIndex].QuestTransform.Length > 0)
            {
                QuestDirectionUI.Instance.SetQuestTransform(questList[questIndex].QuestTransform[0]);
            }
        }
    }

    private void HandleQuestLock()
    {
        if (GetCurrentQuest().QuestLocks != null)
        {
            foreach (QuestLock questlock in GetCurrentQuest().QuestLocks)
            {
                questlock.QuestCompleted();
            }
        }
    }

    public void MadeQuestProgress(Transform completedQuestTransform)
    {
        taskInCurrentQuestCount = questList[questIndex].QuestTransform.Length;
        completedTaskAmountInCurrentQuest++;
        if(completedTaskAmountInCurrentQuest >= taskInCurrentQuestCount)
        {
            CurrentQuestCompleted();
        }
        else
        {
            if (questList[questIndex].QuestTransform.Length > 0)
            {
                if (completedQuestTransform == QuestDirectionUI.Instance.GetCurrentQuestTransform())
                {
                    foreach (Transform questTransform in questList[questIndex].QuestTransform)
                    {
                        if (questTransform != null)
                        {
                            if(completedQuestTransform != questTransform)
                            {
                                QuestDirectionUI.Instance.SetQuestTransform(questTransform);
                                break;
                            }
                        }
                    }
                }
                
            }
        }
    }

    public Quest GetCurrentQuest()
    {
        if(allQuestCompleted)
        {
            return new Quest();
        }
        return questList[questIndex];
    }
    
    public void EnemyKilled()
    {
        if(GetCurrentQuest().Type == Quest.QuestType.Kill)
        {
            killedEnemiesCount++;
            if (QuestUI.Instance != null)
            {
                QuestUI.Instance.SetTitle(GetCurrentQuest().QuestName + "( " + killedEnemiesCount + " )");
            }
            if (killedEnemiesCount >= GetCurrentQuest().enemiesNeededToKill)
            {
                MadeQuestProgress(null);
            }
        }
        else if(GetCurrentQuest().Type == Quest.QuestType.ClearArea)
        {
            killedEnemiesCount++;
            if (QuestUI.Instance != null)
            {
                int leftEnemiesAmount = GetCurrentQuest().enemiesNeededToKill - killedEnemiesCount;
                QuestUI.Instance.SetTitle(GetCurrentQuest().QuestName + "( " + leftEnemiesAmount + " )");
            }
            if (killedEnemiesCount >= GetCurrentQuest().enemiesNeededToKill)
            {
                MadeQuestProgress(null);
            }
        }
        else
        {
            killedEnemiesCount = 0;
        }
    }
}
