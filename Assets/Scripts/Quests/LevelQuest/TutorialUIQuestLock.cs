using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIQuestLock : QuestLock
{
    public override void QuestCompleted()
    {
        TutorialTextUI.Instance.ComletedTutorialEntry();
    }
}
