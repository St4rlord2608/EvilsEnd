using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableScriptableObject")]
public class InteractableScriptableObject : ScriptableObject
{
    public string material;
    public int minMaterialCount;
    public int maxMaterialCount;
    public string interactionHint;
}
