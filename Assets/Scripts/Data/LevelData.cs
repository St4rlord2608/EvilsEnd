using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public string levelName;
    public string followUpLevelName;
    public bool unlocked;
    public bool completed;
}
