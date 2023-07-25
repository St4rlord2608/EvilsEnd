using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPositionUI : MonoBehaviour
{
     public static QuestPositionUI Instance;

    [SerializeField] private Image questPositionImage;

    private void Awake()
    {
        Instance = this;
    }
    public void SetQuestPositionImage(Vector3 questPosition)
    {
        questPositionImage.transform.position = questPosition;
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
