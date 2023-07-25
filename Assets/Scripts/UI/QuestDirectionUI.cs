using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDirectionUI : MonoBehaviour
{
    public static QuestDirectionUI Instance;

    [SerializeField] private Image questPositionImage;
    [SerializeField] private TextMeshProUGUI distanceText;

    private Transform questTransform;
    private Vector3 questPosition;
    private Color defaultQuestColor;
    private Color defaultDistanceColor;

    private void Awake()
    {
        Instance = this;
        defaultQuestColor = questPositionImage.color;
        defaultDistanceColor = distanceText.color;
    }

    private void Update()
    {
        if (questTransform != null)
        {
            questPosition = questTransform.position;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(questPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x >= Screen.width || targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height || targetPositionScreenPoint.z < 0;
            if (!isOffScreen)
            {
                float distance = Vector3.Distance(Camera.main.transform.position, questPosition);
                distance = Mathf.Round(distance);
                questPositionImage.transform.position = targetPositionScreenPoint;
                distanceText.text = distance.ToString();
                questPositionImage.gameObject.SetActive(true);

            }
            else
            {
                questPositionImage.gameObject.SetActive(false);
            }
            if (questPositionImage.gameObject.activeSelf)
            {
                
                Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
                if (Mathf.Abs(screenCenterPoint.x - targetPositionScreenPoint.x) <= 50 && Mathf.Abs(screenCenterPoint.y - targetPositionScreenPoint.y) <= 50)
                {
                    var questColor = defaultQuestColor;
                    questColor.a = 0.01f;
                    questPositionImage.color = questColor;
                    var distanceColor = defaultDistanceColor;
                    distanceColor.a = 0.01f;
                    distanceText.color = distanceColor;
                }else if (Mathf.Abs(screenCenterPoint.x - targetPositionScreenPoint.x) <= 200 && Mathf.Abs(screenCenterPoint.y - targetPositionScreenPoint.y) <= 200)
                {
                    var questColor = defaultQuestColor;
                    questColor.a = 0.2f;
                    questPositionImage.color = questColor;
                    var distanceColor = defaultDistanceColor;
                    distanceColor.a = 0.2f;
                    distanceText.color = distanceColor;
                }
                else
                {
                    questPositionImage.color = defaultQuestColor;
                }
            }
        }
        else
        {
            questPositionImage.gameObject.SetActive(false);
        }
        
    }

    public void SetQuestTransform(Transform questTransform)
    {
        this.questTransform = questTransform;
    }

    public Transform GetCurrentQuestTransform()
    {
        return questTransform;
    }
}
