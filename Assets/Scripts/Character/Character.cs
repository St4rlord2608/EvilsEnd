using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character
{
    public int id;
    public string name;
    public int visualIndex;
    public string status;
    public string sex;
    public bool isPlayer;

    public const string ALIVE_STATUS = "alive";
    public const string DEAD_STATUS = "dead";
    public const string FEMALE_SEX = "female";
    public const string MALE_SEX = "male";

    public Character(int id, string name, int visualIndex, string status, string sex, bool isPlayer = false)
    {
        this.id = id;
        this.name = name;
        this.visualIndex = visualIndex;
        this.status = status;
        this.sex = sex;
        this.isPlayer = isPlayer;
    }
}
