using System.Collections;
using UnityEngine;

public class SpawnBigMan : MonoBehaviour
{
    public GameObject ManPrefab; 
    public Transform player;       
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f; 
    public float spawnInterval = 0.1f;
    private float spawnDelay = 0f; 

    void Start()
    {
        StartCoroutine(SpawnManCoroutine());
    }

    IEnumerator SpawnManCoroutine()
    {
        // Wait for 2 minutes before starting the spawn loop
        yield return new WaitForSeconds(spawnDelay);

        while (true) 
        {
            SpawnManAtRandomPosition();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnManAtRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax); 
        Vector3 spawnPosition = player.position + randomDirection * randomDistance;

        Instantiate(ManPrefab, spawnPosition, Quaternion.identity);
    }
}
