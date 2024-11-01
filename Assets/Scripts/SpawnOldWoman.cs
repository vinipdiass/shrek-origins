using System.Collections;
using UnityEngine;

public class SpawnOldWoman : MonoBehaviour
{
    public GameObject OldWomanPrefab; 
    public Transform player;       
    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f; 
    public float spawnInterval = 0.1f;   

    void Start()
    {
        StartCoroutine(SpawnOldWomanCoroutine());
    }

    IEnumerator SpawnOldWomanCoroutine()
    {
        while (true) 
        {
            SpawnOldWomanAtRandomPosition();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOldWomanAtRandomPosition()
    {
      
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax); 
        Vector3 spawnPosition = player.position + randomDirection * randomDistance;

        Instantiate(OldWomanPrefab, spawnPosition, Quaternion.identity);
    }

}
