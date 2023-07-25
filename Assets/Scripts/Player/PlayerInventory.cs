using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private Dictionary<string, ExploreLoot> exploreLootDictionary;

    private PlayerReloading playerReloading;
    private PlayerWeaponHandler playerWeaponHandler;

    private void Awake()
    {
        Init();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
    }

    private void Start()
    {
        playerReloading = GetComponent<PlayerReloading>();
        foreach (string type in LootTypeNames.GetAllTypeNames())
        {
            AddToInventory(type, ExploreLootDB.GetExploreLoot(type).amount);
        }
        playerReloading.HandleAmmunitionInCurrentMagazine();
    }

    private void Update()
    {
        if(AmmunitionUI.Instance != null)
        {
            AmmunitionUI.Instance.SetAmmunitionInInventory(GetAmountFromInventory(playerWeaponHandler.GetCurrentWeaponType()));
        }
        if(HealingItemsCountUI.Instance != null)
        {
            HealingItemsCountUI.Instance.SetHealingItemsAmount(GetAmountFromInventory(LootTypeNames.HEALTH));
        }
    }

    private void Init()
    {
        exploreLootDictionary = new Dictionary<string, ExploreLoot>();
        foreach (var name in LootTypeNames.GetAllTypeNames())
        {
            exploreLootDictionary[name] = new ExploreLoot()
            {
                type = name,
                amount = 0,
            };
        }
    }
    public void AddToInventory(string type, int amount)
    {
        ExploreLoot exploreLoot = exploreLootDictionary[type];
        exploreLoot.amount += amount;
        ExploreLootDB.UpdateValueOfExploreLoot(exploreLoot);
        exploreLootDictionary[type] = exploreLoot;
    }

    public void RemoveFromInventory(string type, int amount)
    {
        ExploreLoot exploreLoot = exploreLootDictionary[type];
        exploreLoot.amount -= amount;
        ExploreLootDB.UpdateValueOfExploreLoot(exploreLoot);
        exploreLootDictionary[type] = exploreLoot;
    }

    public float GetAmountFromInventory(string type)
    {
        ExploreLoot exploreLoot = exploreLootDictionary[type];
        return exploreLoot.amount;
    }

    public int GetHealingItemCount()
    {
        ExploreLoot exploreLoot = exploreLootDictionary[LootTypeNames.HEALTH];
        return exploreLoot.amount;
    }
}
