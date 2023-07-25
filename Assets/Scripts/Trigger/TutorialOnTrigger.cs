using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOnTrigger : OnTriggerBase
{
    public override void Trigger()
    {
        TutorialTextUI.Instance.ComletedTutorialEntry();
    }
}
