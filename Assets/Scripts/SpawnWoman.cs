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
        spawnRadiusMin = 10f; 
        spawnRadiusMax = 20f;
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
        // Gera uma direção aleatória em um círculo
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Calcula a distância aleatória no intervalo [spawnRadiusMin, spawnRadiusMax]
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // Calcula a posição de spawn
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * randomDistance;

        // Verifica se o inimigo spawnou dentro do jogador e corrige
        if (Vector2.Distance(player.position, spawnPosition) < spawnRadiusMin)
        {
            spawnPosition = (Vector2)player.position + randomDirection * spawnRadiusMin;
        }

        // Instancia o prefab no plano 2D
        Instantiate(WomanPrefab, spawnPosition, Quaternion.identity);
    }
}
