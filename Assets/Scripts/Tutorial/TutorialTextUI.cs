using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTextUI : MonoBehaviour
{
    public static TutorialTextUI Instance;

    [SerializeField] private TutorialText[] tutorialTexts;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    private int tutorialTextIndex = 0;

    private void Awake()
    {
        Instance = this;
        if(tutorialTexts.Length > 0)
        {
            titleText.text = tutorialTexts[0].title;
            contentText.text = tutorialTexts[0].content;
        }
    }

    public void ComletedTutorialEntry()
    {
        tutorialTextIndex++;
        if (tutorialTexts.Length > tutorialTextIndex)
        {
            if (tutorialTexts[tutorialTextIndex].title == "")
            {
                Hide();
            }
            else
            {
                titleText.text = tutorialTexts[tutorialTextIndex].title;
                contentText.text = tutorialTexts[tutorialTextIndex].content;
                Show();
            }
            
        }
        else
        {
            Hide();
        }
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
