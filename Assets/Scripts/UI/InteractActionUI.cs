using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractActionUI : MonoBehaviour
{
    public static InteractActionUI Instance;

    [SerializeField] private TextMeshProUGUI interactActionText;

    private void Awake()
    {
        Instance = this;
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetTextAndActivate(string text)
    {
        interactActionText.text = text;
        Show();
    }
}
