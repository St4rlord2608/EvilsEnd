using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float healthMax = 100f;
    [SerializeField] private AudioClip deathAudioClip;
    
    private float health;

    private bool isDead = false;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        health = healthMax;
        GameInput.Instance.OnHeal += GameInput_OnHeal;
    }

    private void GameInput_OnHeal(object sender, EventArgs e)
    {
        if(playerInventory.GetHealingItemCount() > 0)
        {
            if(health >= healthMax)
            {
                return;
            }
            health += 30f;
            if(health > healthMax)
            {
                health = healthMax;
            }
            if(HealthBarUI.Instance != null)
            {
                HealthBarUI.Instance.TriggerHealthChanged(health / healthMax);
            }
            
            playerInventory.RemoveFromInventory("Health", 1);
        }
    }

    private void Update()
    {
        if(health < 0 && !isDead)
        {
            if(GameInput.Instance != null)
            {
                GameInput.Instance.DeactivateAllGameInputs();
            }
            if(DeathScreenUI.Instance != null)
            {
                DeathScreenUI.Instance.Show();
            }
            if(MusicManager.Instance != null)
            {
                MusicManager.Instance.ActivateScriptedMusicHandling();
                MusicManager.Instance.DirectlyChangeMusicClip(deathAudioClip);
                MusicManager.Instance.DeactivateLoop();
                
            }
            isDead = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DeathPause();
            }
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(DamageIndicatorUI.Instance != null)
        {
            DamageIndicatorUI.Instance.TriggerTookDamage();
        }
        
        if (HealthBarUI.Instance != null)
        {
            HealthBarUI.Instance.TriggerHealthChanged(health / healthMax);
        }
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    private void OnDestroy()
    {
    }

}
