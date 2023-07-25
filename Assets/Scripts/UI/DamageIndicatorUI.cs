using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorUI : MonoBehaviour
{
    public static DamageIndicatorUI Instance;

    [SerializeField] private Image damageIndicator;
    [Space]
    [SerializeField] private float damageIndicatorMaxShowTime = 2f;

    private float currentDamageIndicatorShowTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (damageIndicator.gameObject.activeSelf)
        {
            if (currentDamageIndicatorShowTime >= damageIndicatorMaxShowTime)
            {
                currentDamageIndicatorShowTime = 0;
                damageIndicator.gameObject.SetActive(false);
            }
            else
            {
                currentDamageIndicatorShowTime += Time.deltaTime;
            }
        }
    }

    public void TriggerTookDamage()
    {
        damageIndicator.gameObject.SetActive(true);
        currentDamageIndicatorShowTime = 0;
    }
}
