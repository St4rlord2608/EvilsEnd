using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public static HealthBarUI Instance;

    [SerializeField] private Image barImage;


    public void TriggerHealthChanged(float normalizedHealth)
    {
        barImage.fillAmount = normalizedHealth;
    }
}
