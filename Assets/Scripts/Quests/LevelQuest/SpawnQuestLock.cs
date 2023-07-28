using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnQuestLock : QuestLock
{
    [SerializeField] private Transform spawnPrefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnSpeed = 0.5f;

    private bool spawning = false;
    private float timeSinceLastSpawn = 0f;
    private int currentSpawnAmount = 0;
    public override void QuestCompleted()
    {
        spawning = true;
    }

    private void Update()
    {
        if (spawning && currentSpawnAmount < spawnCount)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if(timeSinceLastSpawn >= spawnSpeed)
            {
                currentSpawnAmount++;
                if(parentTransform != null )
                {
                    Instantiate(spawnPrefab, transform.position, Quaternion.identity, parentTransform);
                }
                else
                {
                    Instantiate(spawnPrefab, transform.position, Quaternion.identity);
                }
                timeSinceLastSpawn = 0f;
            }
        }
        else
        {
            timeSinceLastSpawn = spawnSpeed;
        }
    }
}
