using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected Transform selectedTransform;
    [SerializeField] protected InteractableScriptableObject interactableData;

    protected bool hasBeenInteracted = false;
    protected bool isSelected = false;


    public virtual void SetIsSelected()
    {
        selectedTransform.gameObject.SetActive(true);
        isSelected = true;
    }
    public void SetIsUnselected()
    {
        selectedTransform.gameObject.SetActive(false);
        isSelected = false;
    }

    public abstract void Interact(Transform player);

    public bool GetHasBeenInteracted()
    {
        return hasBeenInteracted;
    }

    public virtual string GetInteractionHint()
    {
        return interactableData.interactionHint;
    }
}
