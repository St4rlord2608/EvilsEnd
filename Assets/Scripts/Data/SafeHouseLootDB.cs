using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SafeHouseLootDB
{
    public static Dictionary<string, SafeHouseLoot> safeHouseLootDictionary;

    public static void Init()
    {
        safeHouseLootDictionary = new Dictionary<string, SafeHouseLoot>();
        foreach (var name in LootTypeNames.GetAllTypeNames())
        {
            safeHouseLootDictionary[name] = new SafeHouseLoot()
            {
                type = name,
                amount = 0,
            };
        }
    }

    public static void UpdateValueOfSafeHouseLoot(SafeHouseLoot loot)
    {
        if (safeHouseLootDictionary == null)
        {
            Init();
        }
        safeHouseLootDictionary[loot.type] = loot;
    }

    public static SafeHouseLoot GetSafeHouseLoot(string type)
    {
        if (safeHouseLootDictionary == null)
        {
            Init();
        }
        return safeHouseLootDictionary[type];
    }

    public static void ClearSafeHouseLoot()
    {
        Init();
    }
}
