using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterUI : MonoBehaviour
{
    public static SelectCharacterUI Instance;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button customizeButton;

    private void Awake()
    {
        Instance = this;
        nextButton.onClick.AddListener(() =>
        {
            SafeHouseCharacterManager.Instance.SelectNextCharacter();
        });

        prevButton.onClick.AddListener(() =>
        {
            SafeHouseCharacterManager.Instance.SelectPreviousCharacter();
        });

        customizeButton.onClick.AddListener(() =>
        {
            Debug.Log("Customize");
        });

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
}
