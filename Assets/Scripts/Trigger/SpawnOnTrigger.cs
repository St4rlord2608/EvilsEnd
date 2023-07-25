using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTrigger : OnTriggerBase
{
    [SerializeField] private Transform spawnPrefab;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnSpeed = 0.5f;

    private bool spawning = false;
    private float timeSinceLastSpawn = 0f;
    private int currentSpawnAmount = 0;
    public override void Trigger()
    {
        spawning = true;
    }

    private void Update()
    {
        if (spawning && currentSpawnAmount <= spawnCount)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnSpeed)
            {
                currentSpawnAmount++;
                Instantiate(spawnPrefab, transform.position, Quaternion.identity);
                timeSinceLastSpawn = 0f;
            }
        }
        else
        {
            timeSinceLastSpawn = spawnSpeed;
        }
    }
}
