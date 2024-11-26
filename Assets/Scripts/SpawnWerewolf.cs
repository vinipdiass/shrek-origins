using System.Collections;
using UnityEngine;

public class SpawnWerewolf : MonoBehaviour
{
    public GameObject WomanPrefab; 
    public Transform player;       
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f; 
    public float spawnInterval = 0.1f;
    private float spawnDelay = 70f; 

    void Start()
    {
        StartCoroutine(SpawnWomanCoroutine());
    }

    IEnumerator SpawnWomanCoroutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (true) 
        {
            SpawnWomanAtRandomPosition();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnWomanAtRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax); 
        Vector3 spawnPosition = player.position + randomDirection * randomDistance;

        Instantiate(WomanPrefab, spawnPosition, Quaternion.identity);
    }
}
