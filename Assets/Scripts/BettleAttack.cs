using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAttack : MonoBehaviour
{
    public GameObject beetlePrefab;      // Prefab do besouro com a animação de voar
    public float rotationSpeed = 200f;   // Velocidade de rotação em graus por segundo
    public float orbitRadius = 2f;       // Raio da órbita ao redor do jogador
    public float damage = 70f;           // Dano causado aos inimigos
    private float baseDamage = 70f;      // Dano base para evolução
    private float attackInterval = 0.5f; // Intervalo entre cada aplicação de dano
    private float attackTimer = 0f;      // Temporizador para o intervalo de ataque

    private int evolution = 0;           // Nível de evolução do ataque

    private GameObject beetleInstance;   // Instância do besouro em cena
    public Transform playerTransform;   // Referência ao transform do jogador

    private float angle = 0f;            // Ângulo atual para calcular a posição na órbita

    private bool isActive = false;       // Se o ataque está ativo ou não

    void Start()
    {
        playerTransform = transform;
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform; // ou atribua de outra forma
        }
        isActive = false; // Inicialmente inativo
        damage = 80f;           // Dano causado aos inimigos
        baseDamage = 80f;
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

        // Cria o besouro a uma distância inicial do jogador
        Vector3 spawnPosition = playerTransform.position + new Vector3(orbitRadius, 0, 0);
        beetleInstance = Instantiate(beetlePrefab, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        if (beetleInstance != null)
        {
            // Atualiza o ângulo para calcular a órbita
            angle += rotationSpeed * Time.deltaTime;
            if (angle >= 360f) angle -= 360f; // Reseta o ângulo para evitar números muito altos

            // Calcula a posição de órbita em relação ao jogador
            float radians = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;

            // Atualiza a posição do besouro ao redor do jogador
            beetleInstance.transform.position = playerTransform.position + offset;

            // Remove a linha que altera a rotação do besouro para que ele mantenha a rotação original
            // Debug para verificar a órbita
            Debug.Log("Orbitando ao redor do jogador. Ângulo: " + angle + " | Posição: " + beetleInstance.transform.position);
            DamageEnemiesInRange();

        }
    }




    private void DamageEnemiesInRange()
    {
        float hitRange = 0.5f; // Alcance para considerar um hit, ajuste conforme necessário

        // Encontra todos os inimigos com a tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            // Checa se o inimigo implementa a interface Damageable
            Damageable damageableEnemy = enemyObj.GetComponent<Damageable>();
            if (damageableEnemy != null)
            {
                float distance = Vector2.Distance(beetleInstance.transform.position, enemyObj.transform.position);
                if (distance <= hitRange)
                {
                    // Aplica dano ao inimigo
                    damageableEnemy.ReceiveDamage(damage, true);
                }
            }
        }
    }

    // Métodos para evolução e atributos
    public void Evolute()
    {
        if (evolution >= 3)
        {
            Debug.Log("Você não pode mais evoluir este poder.");
            return;
        }

        evolution++;
        damage += baseDamage;
        orbitRadius += 0.5f; // Aumenta o raio da órbita
        Debug.Log("Besouro evoluído para o nível " + evolution);
    }

    public void AddAttributeAttack()
    {
        damage += baseDamage;
    }

    public void AddAttributeCooldownReduction()
    {
        // Como o ataque é contínuo, você pode reduzir o intervalo entre ataques
        attackInterval -= 0.1f * attackInterval; // Reduz o intervalo de ataque em 10%
    }
}
