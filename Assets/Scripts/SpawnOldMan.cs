using System.Collections;
using UnityEngine;

public class SpawnOldMan : MonoBehaviour
{
    public GameObject oldManPrefab;
    public Transform player;
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f;
    public float spawnInterval = 0.1f;

    void Start()
    {
        StartCoroutine(SpawnOldManCoroutine());
        spawnRadiusMax = 10f;
        spawnRadiusMin = 20f;
    }

    IEnumerator SpawnOldManCoroutine()
    {
        while (true)
        {
            SpawnOldManAtRandomPosition();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOldManAtRandomPosition()
    {
    Vector3 randomDirection = Random.onUnitSphere;
    float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);
    Vector3 spawnPosition = player.position + randomDirection * randomDistance;

    Instantiate(oldManPrefab, spawnPosition, Quaternion.identity);
    }


}
