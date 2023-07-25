using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExploreLootDB
{
    public static Dictionary<string, ExploreLoot> exploreLootDictionary;

    public static void Init()
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

    public static void UpdateValueOfExploreLoot(ExploreLoot loot)
    {
        if(exploreLootDictionary == null)
        {
            Init();
        }
        exploreLootDictionary[loot.type] = loot;
    }

    public static ExploreLoot GetExploreLoot(string type)
    {
        if(exploreLootDictionary == null)
        {
            Init();
        }
        return exploreLootDictionary[type];
    }

    public static void ClearExploreLoot()
    {
        Init();
    }
}
