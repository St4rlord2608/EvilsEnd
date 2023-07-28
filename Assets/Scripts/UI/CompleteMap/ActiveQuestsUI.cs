using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveQuestsUI : MonoBehaviour
{
    public static ActiveQuestsUI Instance;

    [SerializeField] private TextMeshProUGUI activeQuestText;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void SetActiveQuest(WorldQuest worldQuest)
    {
        activeQuestText.text = worldQuest.questName;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
