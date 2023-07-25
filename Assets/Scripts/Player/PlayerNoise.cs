using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    [SerializeField] private float defaultNoise = 2f;
    private PlayerMovement playerMovement;
    private PlayerShootingHandler playerShootingHandler;
    private Collider[] nearbyColliders;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShootingHandler = GetComponent<PlayerShootingHandler>();
    }
    private void Update()
    {
        HandleNoise();
    }

    private void HandleNoise()
    {
        float currentNoise = defaultNoise;
        if (playerShootingHandler.GetCurrentShootingNoise() > playerMovement.GetCurrentMovementNoise())
        {
            currentNoise = playerShootingHandler.GetCurrentShootingNoise();
        }
        else if(playerMovement.GetCurrentMovementNoise() > 0)
        {
            currentNoise = playerMovement.GetCurrentMovementNoise();
        }
        nearbyColliders = Physics.OverlapSphere(transform.position, currentNoise);
        foreach (Collider collider in nearbyColliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.HearsPlayer(transform);
            }
        }
    }
}
