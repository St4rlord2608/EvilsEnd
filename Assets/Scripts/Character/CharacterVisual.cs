using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterVisual
{
    public enum Sex
    {
        female,
        male
    }
    public string name;
    public Sex sex;
}
