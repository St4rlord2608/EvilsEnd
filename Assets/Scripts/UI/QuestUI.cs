using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    [SerializeField] private TextMeshProUGUI questTitle;

    private Quest currentQuest;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(QuestHandler.Instance != null)
        {
            currentQuest = QuestHandler.Instance.GetCurrentQuest();
            SetTitle(currentQuest.QuestName);
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void SetTitle(string title)
    {
        questTitle.text = title;
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
