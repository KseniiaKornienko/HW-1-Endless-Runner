using UnityEngine;
using System.Collections.Generic;

public class BonusSpawner : MonoBehaviour
{
    public GameObject healthBonusPrefab; 
    public GameObject shieldBonusPrefab; 
    public Transform player; 
    public float spawnDistance = 30f; 
    public float spawnY = 1.3f; 

    private float[] lanes = { -3.3f, 0f, 3.3f };
    private float nextSpawnZ = 0f;
    private GameObject bonus;
    private List<GameObject> spawnedBonuses = new List<GameObject>();

    void Start()
    {
        nextSpawnZ = player.position.z + spawnDistance;
    }

    void Update()
    {
        if (player.position.z > nextSpawnZ - spawnDistance)
        {
            if (Random.Range(0f, 1f) < 0.05f) {
                SpawnBonus();
            } 
        nextSpawnZ += Random.Range(5f, 15f); 
        }
        CleanupBonuses();
    }

    void SpawnBonus()
    {
        int randomLane = Random.Range(0, 3);
        float xPos = lanes[randomLane];
        
        Vector3 spawnPos = new Vector3(xPos, spawnY, nextSpawnZ);
        if (Random.Range(0f, 1f) < 0.5f) {
            bonus = Instantiate(healthBonusPrefab, spawnPos, Quaternion.identity);
        } else {
            bonus = Instantiate(shieldBonusPrefab, spawnPos, Quaternion.identity);
        }
        spawnedBonuses.Add(bonus);
    }

    void CleanupBonuses()
    {
        for (int i = spawnedBonuses.Count - 1; i >= 0; i--)
        {
            if (spawnedBonuses[i] == null)
            {
                spawnedBonuses.RemoveAt(i);
                continue;
            }
            
            if (spawnedBonuses[i].transform.position.z < player.position.z - 10f)
            {
                Destroy(spawnedBonuses[i]);
                spawnedBonuses.RemoveAt(i);
            }
        }
    }
}
