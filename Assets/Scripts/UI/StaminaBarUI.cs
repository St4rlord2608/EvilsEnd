using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{

    public static StaminaBarUI Instance;
    [SerializeField] private Image barImage;

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerStaminaChanged(float normalizedStamina)
    {
        barImage.fillAmount = normalizedStamina;
    }
}
