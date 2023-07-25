using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouseInventoryManager : MonoBehaviour
{
    public static SafeHouseInventoryManager Instance;

    private Dictionary<string, ExploreLoot> exploreLootDictionary;
    private Dictionary<string, SafeHouseLoot> safeHouseLootDictionary;

    //private int assaultRifleAmmunition;
    //private int shotgunAmmunition;
    //private int heavySniperAmmunition;
    //private int bazookaAmmunition;
    //private int dmrAmmunition;
    //private int submachineAmmunition;
    //private int buildingMaterialCount;
    //private int chemicalCount;
    //private int healingItemCount;

    //private int exploreAssaultRifleAmmunition;
    //private int exploreShotgunAmmunition;
    //private int exploreHeavySniperAmmunition;
    //private int exploreBazookaAmmunition;
    //private int exploreDmrAmmunition;
    //private int exploreSubmachineAmmunition;
    //private int exploreBuildingMaterialCount;
    //private int exploreChemicalCount;
    //private int exploreHealingItemCount;

    private void Awake()
    {
        Instance = this;
        Init();
        foreach (var type in LootTypeNames.GetAllTypeNames())
        {
            int amount = ExploreLootDB.GetExploreLoot(type).amount;
            int currentSafeHouseAmount = SafeHouseLootDB.GetSafeHouseLoot(type).amount;
            AddToInventory(type, currentSafeHouseAmount + amount);
        }
        ExploreLootDB.ClearExploreLoot();
    }

    private void Start()
    {
        
    }

    private void Init()
    {
        exploreLootDictionary = new Dictionary<string, ExploreLoot>();
        safeHouseLootDictionary = new Dictionary<string, SafeHouseLoot>();
        foreach (var name in LootTypeNames.GetAllTypeNames())
        {
            exploreLootDictionary[name] = new ExploreLoot()
            {
                type = name,
                amount = 0,
            };
            safeHouseLootDictionary[name] = new SafeHouseLoot()
            {
                type = name,
                amount = 0,
            };
        }
    }

    public void AddToInventory(string type, int amount)
    {
        SafeHouseLoot safeHouseLoot = safeHouseLootDictionary[type];
        safeHouseLoot.amount += amount;
        SafeHouseLootDB.UpdateValueOfSafeHouseLoot(safeHouseLoot);
        safeHouseLootDictionary[type] = safeHouseLoot;
        //switch (type)
        //{
        //    case LootTypeNames.HEALTH:
        //        healingItemCount += amount;
        //        break;
        //    case LootTypeNames.BUILDING_MATERIAL:
        //        buildingMaterialCount += amount;
        //        break;
        //    case LootTypeNames.CHEMICAL:
        //        chemicalCount += amount;
        //        break;
        //    case LootTypeNames.ASSAULT_RIFLE:
        //        assaultRifleAmmunition += amount;
        //        break;
        //    case LootTypeNames.HEAVY_SNIPER:
        //        heavySniperAmmunition += amount;
        //        break;
        //    case LootTypeNames.SHOTGUN:
        //        shotgunAmmunition += amount;
        //        break;
        //    case LootTypeNames.BAZOOKA:
        //        bazookaAmmunition += amount;
        //        break;
        //    case LootTypeNames.DMR:
        //        dmrAmmunition += amount;
        //        break;
        //    case LootTypeNames.MP:
        //        submachineAmmunition += amount;
        //        break;
        //}
    }

    public void RemoveFromInventory(string type, int amount)
    {
        SafeHouseLoot safeHouseLoot = safeHouseLootDictionary[type];
        safeHouseLoot.amount -= amount;
        SafeHouseLootDB.UpdateValueOfSafeHouseLoot(safeHouseLoot);
        safeHouseLootDictionary[type] = safeHouseLoot;
        //switch (type)
        //{
        //    case LootTypeNames.HEALTH:
        //        healingItemCount -= amount;
        //        break;
        //    case LootTypeNames.BUILDING_MATERIAL:
        //        buildingMaterialCount -= amount;
        //        break;
        //    case LootTypeNames.CHEMICAL:
        //        chemicalCount -= amount;
        //        break;
        //    case LootTypeNames.ASSAULT_RIFLE:
        //        assaultRifleAmmunition -= amount;
        //        break;
        //    case LootTypeNames.HEAVY_SNIPER:
        //        heavySniperAmmunition -= amount;
        //        break;
        //    case LootTypeNames.SHOTGUN:
        //        shotgunAmmunition -= amount;
        //        break;
        //    case LootTypeNames.BAZOOKA:
        //        bazookaAmmunition -= amount;
        //        break;
        //    case LootTypeNames.DMR:
        //        dmrAmmunition -= amount;
        //        break;
        //    case LootTypeNames.MP:
        //        submachineAmmunition -= amount;
        //        break;
        //}
    }

    public float GetAmountFromInventory(string type)
    {
        SafeHouseLoot safeHouseLoot = safeHouseLootDictionary[type];
        return safeHouseLoot.amount;
        //switch (type)
        //{
        //    case LootTypeNames.HEALTH:
        //        return healingItemCount;
        //    case LootTypeNames.BUILDING_MATERIAL:
        //        return buildingMaterialCount;
        //    case LootTypeNames.CHEMICAL:
        //        return chemicalCount;
        //    case LootTypeNames.ASSAULT_RIFLE:
        //        return assaultRifleAmmunition;
        //    case LootTypeNames.HEAVY_SNIPER:
        //        return heavySniperAmmunition;
        //    case LootTypeNames.SHOTGUN:
        //        return shotgunAmmunition;
        //    case LootTypeNames.BAZOOKA:
        //        return bazookaAmmunition;
        //    case LootTypeNames.DMR:
        //        return dmrAmmunition;
        //    case LootTypeNames.MP:
        //        return submachineAmmunition;
        //    default:
        //        return 0;

        //}
    }

    public void AddToExploreInventory(string type, int amount)
    {
        ExploreLoot exploreLoot = exploreLootDictionary[type];
        exploreLoot.amount += amount;
        ExploreLootDB.UpdateValueOfExploreLoot(exploreLoot);
        exploreLootDictionary[type] = exploreLoot;
        //switch (type)
        //{
        //    case LootTypeNames.HEALTH:
        //        exploreHealingItemCount += amount;
        //        break;
        //    case LootTypeNames.BUILDING_MATERIAL:
        //        exploreBuildingMaterialCount += amount;
        //        break;
        //    case LootTypeNames.CHEMICAL:
        //        exploreChemicalCount += amount;
        //        break;
        //    case LootTypeNames.ASSAULT_RIFLE:
        //        exploreAssaultRifleAmmunition += amount;
        //        break;
        //    case LootTypeNames.HEAVY_SNIPER:
        //        exploreHeavySniperAmmunition += amount;
        //        break;
        //    case LootTypeNames.SHOTGUN:
        //        exploreShotgunAmmunition += amount;
        //        break;
        //    case LootTypeNames.BAZOOKA:
        //        exploreBazookaAmmunition += amount;
        //        break;
        //    case LootTypeNames.DMR:
        //        exploreDmrAmmunition += amount;
        //        break;
        //    case LootTypeNames.MP:
        //        exploreSubmachineAmmunition += amount;
        //        break;
        //}
    }

    public void RemoveFromExploreInventory(string type, int amount)
    {
        ExploreLoot exploreLoot = exploreLootDictionary[type];
        exploreLoot.amount -= amount;
        ExploreLootDB.UpdateValueOfExploreLoot(exploreLoot);
        exploreLootDictionary[type] = exploreLoot;
        //switch (type)
        //{
        //    case LootTypeNames.HEALTH:
        //        exploreHealingItemCount -= amount;
        //        break;
        //    case LootTypeNames.BUILDING_MATERIAL:
        //        exploreBuildingMaterialCount -= amount;
        //        break;
        //    case LootTypeNames.CHEMICAL:
        //        exploreChemicalCount -= amount;
        //        break;
        //    case LootTypeNames.ASSAULT_RIFLE:
        //        exploreAssaultRifleAmmunition -= amount;
        //        break;
        //    case LootTypeNames.HEAVY_SNIPER:
        //        exploreHeavySniperAmmunition -= amount;
        //        break;
        //    case LootTypeNames.SHOTGUN:
        //        exploreShotgunAmmunition -= amount;
        //        break;
        //    case LootTypeNames.BAZOOKA:
        //        exploreBazookaAmmunition -= amount;
        //        break;
        //    case LootTypeNames.DMR:
        //        exploreDmrAmmunition -= amount;
        //        break;
        //    case LootTypeNames.MP:
        //        exploreSubmachineAmmunition -= amount;
        //        break;
        //}
    }

    public float GetAmountFromExploreInventory(string type)
    {
        ExploreLoot exploreLoot = exploreLootDictionary[type];
        return exploreLoot.amount;
        //switch (type)
        //{
        //    case LootTypeNames.HEALTH:
        //        return exploreHealingItemCount;
        //    case LootTypeNames.BUILDING_MATERIAL:
        //        return exploreBuildingMaterialCount;
        //    case LootTypeNames.CHEMICAL:
        //        return exploreChemicalCount;
        //    case LootTypeNames.ASSAULT_RIFLE:
        //        return exploreAssaultRifleAmmunition;
        //    case LootTypeNames.HEAVY_SNIPER:
        //        return exploreHeavySniperAmmunition;
        //    case LootTypeNames.SHOTGUN:
        //        return exploreShotgunAmmunition;
        //    case LootTypeNames.BAZOOKA:
        //        return exploreBazookaAmmunition;
        //    case LootTypeNames.DMR:
        //        return exploreDmrAmmunition;
        //    case LootTypeNames.MP:
        //        return exploreSubmachineAmmunition;
        //    default:
        //        return 0;

        //}
    }
}
