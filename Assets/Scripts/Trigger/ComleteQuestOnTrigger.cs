using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComleteQuestOnTrigger : OnTriggerBase
{
    public override void Trigger()
    {
        if(QuestHandler.Instance != null)
        {
            QuestHandler.Instance.MadeQuestProgress(null);
        }
    }
}
