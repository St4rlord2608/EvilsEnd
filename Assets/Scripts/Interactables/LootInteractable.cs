using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInteractable : Interactable
{
    [SerializeField] private CollectedUI collectedUI;
    [SerializeField] private Transform emptyInteractablePrefab;

    private PlayerInventory playerInventory;
    private int materialAmount;

    private void Start()
    {
        selectedTransform.gameObject.SetActive(false);
        materialAmount = (int)Random.Range(interactableData.minMaterialCount,
                   interactableData.maxMaterialCount);
    }

    public override void Interact(Transform player)
    {
        if (!hasBeenInteracted)
        {
            if (player.TryGetComponent<PlayerInventory>(out playerInventory))
            {
                playerInventory.AddToInventory(interactableData.material, materialAmount);
                if (emptyInteractablePrefab != null)
                {
                    Transform emptyInteractableTransform = Instantiate(emptyInteractablePrefab, transform.position, transform.rotation);
                    EmptyInteractable emptyInteractable = emptyInteractableTransform.GetComponent<EmptyInteractable>();
                    emptyInteractable.SetMaterialAndAmount(interactableData.material, materialAmount);
                    Destroy(gameObject);
                    return;
                }
                if(collectedUI != null)
                {
                    collectedUI.SetMaterialAndAmount(interactableData.material, materialAmount);
                }
                hasBeenInteracted = true;
            }
        }
        selectedTransform.gameObject.SetActive(false);
    }
}
