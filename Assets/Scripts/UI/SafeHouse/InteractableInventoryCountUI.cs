using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableInventoryCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetTypeText(string type)
    {
        typeText.text = type;
    }

    public void SetAmountText(string amount)
    {
        amountText.text = amount;
    }
}
