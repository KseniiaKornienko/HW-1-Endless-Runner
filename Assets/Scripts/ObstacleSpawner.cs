using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject wallObstaclePrefab; 
    public GameObject sphereObstaclePrefab; 
    public Transform player; 
    public float spawnDistance = 30f; 
    public float spawnY = 0.7f; 

    private float[] lanes = { -3.3f, 0f, 3.3f };
    private float nextSpawnZ = 0f;
    private GameObject obstacle;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        nextSpawnZ = player.position.z + spawnDistance;
    }

    void Update()
    {
        if (player.position.z > nextSpawnZ - spawnDistance)
        {
            if (Random.Range(0f, 1f) < 0.8f) {
                SpawnObstacle();
            } 
        nextSpawnZ += Random.Range(5f, 15f); 
        }
        CleanupObstacles();
    }

    void SpawnObstacle()
    {
        int randomLane = Random.Range(0, 3);
        float xPos = lanes[randomLane];
        
        Vector3 spawnPos = new Vector3(xPos, spawnY, nextSpawnZ);
        if (Random.Range(0f, 1f) < 0.5f) {
            obstacle = Instantiate(wallObstaclePrefab, spawnPos, Quaternion.identity);
        } else {
            obstacle = Instantiate(sphereObstaclePrefab, spawnPos, Quaternion.identity);
        }
        spawnedObstacles.Add(obstacle);
    }

    void CleanupObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i] == null)
            {
                spawnedObstacles.RemoveAt(i);
                continue;
            }
            
            if (spawnedObstacles[i].transform.position.z < player.position.z - 10f)
            {
                Destroy(spawnedObstacles[i]);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }
}