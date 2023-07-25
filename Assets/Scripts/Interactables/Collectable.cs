using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableScriptableObject collectableData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            playerInventory.AddToInventory(collectableData.Type, collectableData.Amount);
            Destroy(this.gameObject);
        }
    }
}
