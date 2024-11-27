using System.Collections.Generic;
using UnityEngine;

public class BeetleAttack : MonoBehaviour
{
    public GameObject beetlePrefab;
    public float rotationSpeed = 200f;
    public float orbitRadius = 2f;
    public float damage = 70f;
    private float baseDamage = 70f;
    private float attackInterval = 0.5f;
    private float attackTimer = 0f;

    private int evolution = -1;

    private List<GameObject> beetleInstances = new List<GameObject>(); // Lista para armazenar instâncias de besouros
    private List<float> angles = new List<float>();                   // Lista para armazenar ângulos individuais

    public Transform playerTransform;

    private bool isActive = false;

    void Start()
    {
        playerTransform = transform;
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
        isActive = false;
        damage = 50f;
        baseDamage = 50f;
        orbitRadius = 2f;
        rotationSpeed = 100f;
    }

    public void ActivateBeetle()
    {
        if (!isActive)
        {
            isActive = true;
            SpawnBeetle();
        }
    }

    private void SpawnBeetle()
    {
        if (playerTransform == null)
        {
            Debug.LogError("playerTransform não está atribuído!");
            return;
        }

        // Calcula o ângulo de separação com base no número total de besouros
        float angleSeparation = 360f / (beetleInstances.Count + 1); // +1 pois estamos adicionando um novo besouro

        // Ajusta os ângulos para distribuir igualmente os besouros existentes e o novo
        angles.Clear(); // Limpa os ângulos anteriores para recalculá-los

        for (int i = 0; i <= beetleInstances.Count; i++) // <= para incluir o novo besouro
        {
            angles.Add(i * angleSeparation); // Distribui os ângulos uniformemente
        }

        // Cria e posiciona o novo besouro
        Vector3 spawnPosition = playerTransform.position + new Vector3(orbitRadius, 0, 0);
        GameObject beetle = Instantiate(beetlePrefab, spawnPosition, Quaternion.identity);
        beetleInstances.Add(beetle);

        // Ajusta a posição inicial do novo besouro
        float radians = angles[beetleInstances.Count - 1] * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;
        beetle.transform.position = playerTransform.position + offset;
    }


    void Update()
    {
        for (int i = 0; i < beetleInstances.Count; i++)
        {
            GameObject beetle = beetleInstances[i];

            if (beetle != null)
            {
                // Atualiza o ângulo para cada besouro
                angles[i] += rotationSpeed * Time.deltaTime;
                if (angles[i] >= 360f) angles[i] -= 360f;

                float radians = angles[i] * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;

                beetle.transform.position = playerTransform.position + offset;

                beetle.transform.eulerAngles = new Vector3(
                    beetle.transform.eulerAngles.x,
                    playerTransform.eulerAngles.y,
                    beetle.transform.eulerAngles.z
                );
            }
        }

        DamageEnemiesInRange();
    }

    private void DamageEnemiesInRange()
    {
        float hitRange = 0.7f;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            Damageable damageableEnemy = enemyObj.GetComponent<Damageable>();
            if (damageableEnemy != null)
            {
                foreach (GameObject beetle in beetleInstances)
                {
                    float distance = Vector2.Distance(beetle.transform.position, enemyObj.transform.position);
                    if (distance <= hitRange)
                    {
                        damageableEnemy.ReceiveDamage(damage, true);
                        break;
                    }
                }
            }
        }
    }

    public void Evolute()
    {
        if (evolution >= 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
            return;
        }
        else
        {
            evolution++;
            //rotationSpeed += 50;

            // Adiciona um besouro extra a cada evolução
            SpawnBeetle();
            Debug.Log("Besouro evoluído para o nível " + evolution);
        }
    }

    public void AddAttributeAttack()
    {
        damage += baseDamage;
    }

    public void AddAttributeCooldownReduction()
    {
        rotationSpeed += 50;
    }
}
