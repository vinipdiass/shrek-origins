using System.Collections;
using UnityEngine;

public class SpawnWerewolfBoss : MonoBehaviour
{
    public GameObject BoyPrefab; 
    public Transform player;       
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f; 
    public float spawnInterval = 0.1f;
    private float spawnDelay = 200f; 

    void Start()
    {
        StartCoroutine(SpawnBoyCoroutine());
    }

    IEnumerator SpawnBoyCoroutine()
    {
        // Wait for 2 minutes before starting the spawn loop
        yield return new WaitForSeconds(spawnDelay);

        while (true) 
        {
            SpawnBoyAtRandomPosition();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBoyAtRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax); 
        Vector3 spawnPosition = player.position + randomDirection * randomDistance;

        Instantiate(BoyPrefab, spawnPosition, Quaternion.identity);
    }
}
