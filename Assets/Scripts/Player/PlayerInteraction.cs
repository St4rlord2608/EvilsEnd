using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Transform interactRayStartingPosition;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform LookDirectionTransform;

    private Interactable lastInteractable;

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if(lastInteractable != null && !lastInteractable.GetHasBeenInteracted())
        {
            lastInteractable.Interact(transform);
        }
    }

    private void Update()
    {
        Vector3 characterHeight = new Vector3(0, characterController.height, 0);
        InteractActionUI.Instance.Hide();
        if (Physics.CapsuleCast(transform.position, transform.position + characterHeight,0.1f, LookDirectionTransform.forward,  out RaycastHit hit, 1f, interactableLayers))
        {
            if(hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
            {
                if(lastInteractable != null && interactable.gameObject != lastInteractable.gameObject)
                {
                    lastInteractable.SetIsUnselected();
                }
                if(!interactable.GetHasBeenInteracted())
                {
                    interactable.SetIsSelected();
                    InteractActionUI.Instance.SetTextAndActivate(interactable.GetInteractionHint());
                    lastInteractable = interactable;
                }
                else
                {
                    InteractActionUI.Instance.Hide();
                }
            }
        }
        else
        {
            if(lastInteractable != null)
            {
                lastInteractable.SetIsUnselected();
                lastInteractable = null;
                InteractActionUI.Instance.Hide();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + LookDirectionTransform.forward, characterController.radius);
    }
}
