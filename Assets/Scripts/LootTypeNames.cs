using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootTypeNames
{
    public const string ASSAULT_RIFLE = "Assault Rifle";
    public const string HEAVY_SNIPER = "Heavy Sniper";
    public const string BAZOOKA = "Bazooka";
    public const string SHOTGUN = "Shotgun";
    public const string DMR = "DMR";
    public const string MP = "MP";
    public const string BUILDING_MATERIAL = "Building Material";
    public const string CHEMICAL = "Chemical";
    public const string HEALTH = "Health";

    public static List<string> names;

    public static List<string> GetAllTypeNames()
    {
        names = new List<string>
        {
            ASSAULT_RIFLE,
            HEAVY_SNIPER,
            BAZOOKA,
            SHOTGUN,
            DMR,
            MP,
            BUILDING_MATERIAL,
            CHEMICAL,
            HEALTH
        };
        return names;
    }
}
