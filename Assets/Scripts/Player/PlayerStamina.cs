using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 10f;
    [SerializeField] private float staminaRegenarationCooldown = 1f;
    [SerializeField] private float minStaminaAmountAfterCompleteRunOut = 1f;
    [SerializeField] private float staminaRegenerationSpeed = 2f;

    private PlayerMovement playerMovement;
    private float currentStamina;
    private float currentStaminaRegenerationCooldown = 0;
    private bool ranOutOfStamina = false;
    private bool hasStamina;

    private void Awake()
    {
        currentStamina = maxStamina;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (currentStamina <= 0)
        {
            ranOutOfStamina = true;
        }
        if (!ranOutOfStamina)
        {
            hasStamina = true;
        }
        else
        {
            if(currentStamina >= minStaminaAmountAfterCompleteRunOut)
            {
                ranOutOfStamina = false;
            }
            hasStamina = false;
        }
        
        if (playerMovement.IsSprinting())
        {
            currentStaminaRegenerationCooldown = staminaRegenarationCooldown;
            currentStamina -= Time.deltaTime;
            if(StaminaBarUI.Instance != null)
            {
                StaminaBarUI.Instance.TriggerStaminaChanged(currentStamina / maxStamina);
            }
        }
        else
        {
            if(currentStaminaRegenerationCooldown > 0)
            {
                currentStaminaRegenerationCooldown -= Time.deltaTime;
            }
            if(currentStaminaRegenerationCooldown <= 0 && currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime * staminaRegenerationSpeed;
                if (StaminaBarUI.Instance != null)
                {
                    StaminaBarUI.Instance.TriggerStaminaChanged(currentStamina / maxStamina);
                }
            }
        }
    }

    public bool HasStamina()
    {
        return hasStamina;
    }
}
