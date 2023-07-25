using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXNoiseHandler : MonoBehaviour
{
    [SerializeField] private float spawnNoise = 3f;

    private void Awake()
    {
        Collider[] inNoiseRangeColliders = Physics.OverlapSphere(transform.position, spawnNoise);
        foreach (Collider collider in inNoiseRangeColliders)
        {

            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.HearsSomething(transform);
            }
        }
    }
}
