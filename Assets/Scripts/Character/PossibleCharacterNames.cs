using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PossibleCharacterNames
{
    public static List<string> femaleNames = new List<string> { 
    "Jessica",
    "Agatha",
    "Selene",
    "Emely",
    "Julia",
    "Zandaria",
    "Zoe"
    };
    public static List<string> maleNames = new List<string> { 
    "Thomas",
    "Theodore",
    "Martin",
    "Kevin",
    "Tom",
    "Kratos",
    "Gavin"
    };

    public static string GetRandomMaleName()
    {
        int nameIndex = Random.Range(0, maleNames.Count);
        return maleNames[nameIndex];
    }

    public static string GetRandomFemaleName()
    {
        int nameIndex = Random.Range(0, femaleNames.Count);
        return femaleNames[nameIndex];
    }
}
