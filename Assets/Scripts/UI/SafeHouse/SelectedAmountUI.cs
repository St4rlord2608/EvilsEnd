using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedAmountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;

    private void Awake()
    {
        Hide();
    }
    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }
    public void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
