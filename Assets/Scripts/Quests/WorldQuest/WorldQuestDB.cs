using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldQuestDB
{
    public static List<WorldQuest> quests = new List<WorldQuest>();

    public static void SetQuests(List<WorldQuest> quests)
    {
        WorldQuestDB.quests = quests;
    }

    public static List<WorldQuest> GetQuests()
    {
        return quests;
    }
}
