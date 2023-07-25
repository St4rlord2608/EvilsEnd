using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CollectableScriptableObject")]
public class CollectableScriptableObject : ScriptableObject
{
    public string Type;
    public int Amount;
}
