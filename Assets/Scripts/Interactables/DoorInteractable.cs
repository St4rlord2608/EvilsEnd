using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact(Transform player)
    {
        animator.SetTrigger("ChangeDoorState");
    }

}
