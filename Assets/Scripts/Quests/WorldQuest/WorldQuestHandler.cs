using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldQuestHandler
{
    private static List<int> completedQuestIds = new List<int>();
    private static List<int> failedQuestIds = new List<int>();

    public static event EventHandler OnQuestCompleted;
    public  static event EventHandler OnQuestFailed;

    public static void CompletedQuest(int questId)
    {
        completedQuestIds.Add(questId);
        OnQuestCompleted?.Invoke(null, EventArgs.Empty);
    }

    public static void FailedQuest(int questId)
    {
        failedQuestIds.Add(questId);
        OnQuestFailed?.Invoke(null, EventArgs.Empty);
    }

    public static List<int> GetAllCompletedQuestIds()
    {
        return completedQuestIds;
    }

    public static List<int> GetAllFailedQuestIds() 
    { 
        return failedQuestIds; 
    }

    public static void ClearCompletedQuestIds()
    {
        completedQuestIds.Clear();
    }

    public static void ClearFailedQuestIds()
    {
        failedQuestIds.Clear();
    }
}
