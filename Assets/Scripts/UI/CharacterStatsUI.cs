using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;

    private void Start()
    {
        Hide();
    }

    public void SetName(string name)
    {
        nameText.text = name;
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
