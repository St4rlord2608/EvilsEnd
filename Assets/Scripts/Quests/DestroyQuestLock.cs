using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyQuestLock : QuestLock
{
    public override void QuestCompleted()
    {
        Destroy(gameObject);
    }
}
