using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmptyInteractable : MonoBehaviour
{
    [SerializeField] private CollectedUI collectedUI;
    public void SetMaterialAndAmount(string material, int materialAmount)
    {
        collectedUI.SetMaterialAndAmount(material, materialAmount);
    }
}
