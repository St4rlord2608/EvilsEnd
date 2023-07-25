using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealingItemsCountUI : MonoBehaviour
{

    public static HealingItemsCountUI Instance;
    [SerializeField] private TextMeshProUGUI amountText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetHealingItemsAmount(float amount)
    {
        amountText.text = amount.ToString();
    }
}
