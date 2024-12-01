using System.Collections;
using UnityEngine;

public class SpawnWoman : MonoBehaviour
{
    public GameObject WomanPrefab;
    public Transform player;
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f;
    public float spawnInterval = 0.1f;
    private float spawnDelay = 50f;

    void Start()
    {
        StartCoroutine(SpawnWomanCoroutine());
        spawnRadiusMax = 30f;
        spawnRadiusMin = 30f;
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
        Vector3 randomDirection = Random.onUnitSphere;
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);
        Vector3 spawnPosition = player.position + randomDirection * randomDistance;

        Instantiate(WomanPrefab, spawnPosition, Quaternion.identity);
    }
}
