using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractable : Interactable
{
    [SerializeField] private int maxInteractionAmount = 1;
    [SerializeField] private string interactionHintText;
    [SerializeField] private Interactable actualInteraction;

    private int currentInteractionCount = 0;
    public override void SetIsSelected()
    {
        Quest currentQuest = QuestHandler.Instance.GetCurrentQuest();
        if (currentQuest.QuestTransform == null)
        {
            SetIsUnselected();
            return;
        }
        foreach (Transform questTransform in currentQuest.QuestTransform)
        {
            if (questTransform == transform)
            {
                selectedTransform.gameObject.SetActive(true);
                isSelected = true;
            }
        }
        
    }
    public override void Interact(Transform player)
    {
        if(!isSelected)
        {
            return;
        }
        currentInteractionCount++;
        if(QuestHandler.Instance != null)
        {
            QuestHandler.Instance.MadeQuestProgress(transform);
            SetIsUnselected();
            if(currentInteractionCount >= maxInteractionAmount)
            {
                if (actualInteraction != null)
                {
                    actualInteraction.Interact(player);
                }
                Destroy(this);
            }
        }
    }

    public override string GetInteractionHint()
    {
        Quest currentQuest = QuestHandler.Instance.GetCurrentQuest();
        if(currentQuest.QuestTransform == null)
        {
            return "";
        }
        foreach (Transform questTransform in currentQuest.QuestTransform)
        {
            if (questTransform == transform)
            {
                return interactionHintText;
            }
        }
        return "";
    }
}
