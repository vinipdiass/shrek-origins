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
        spawnRadiusMax = 30f;
        spawnRadiusMin = 30f;
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

    Instantiate(ManPrefab, spawnPosition, Quaternion.identity);
    }


}
