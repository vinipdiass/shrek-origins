using System.Collections;
using UnityEngine;

public class SpawnGirl : MonoBehaviour
{
    public GameObject GirlPrefab;
    public Transform player;
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f;
    public float spawnInterval = 0.1f;
    private float spawnDelay = 160f;

    void Start()
    {
        StartCoroutine(SpawnGirlCoroutine());
        spawnRadiusMax = 10f;
        spawnRadiusMin = 20f;
    }

    IEnumerator SpawnGirlCoroutine()
    {
        // Wait for 2 minutes before starting the spawn loop
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            SpawnGirlAtRandomPosition();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGirlAtRandomPosition()
    {
    Vector3 randomDirection = Random.onUnitSphere;
    float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);
    Vector3 spawnPosition = player.position + randomDirection * randomDistance;

    Instantiate(GirlPrefab, spawnPosition, Quaternion.identity);
    }
}
