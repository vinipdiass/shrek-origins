using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public Transform targetEnemy;
    private Bounds projectileBounds;

    private int phase = 0; // 0: indo para o alvo, 1: primeira direção aleatória, 2: segunda direção aleatória
    private Vector2 currentDirection;
    private float phaseTimer = 0f;
    public float hitRadius = 0.5f; // Distância para considerar o hit em inimigos no caminho

    void Start()
    {
        if (targetEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        float projectileMargin = 0.5f;
        projectileBounds = new Bounds();
        projectileBounds.SetMinMax(
            new Vector3(Globals.WorldBounds.min.x - projectileMargin, Globals.WorldBounds.min.y - projectileMargin, 0.0f),
            new Vector3(Globals.WorldBounds.max.x + projectileMargin, Globals.WorldBounds.max.y + projectileMargin, 0.0f)
        );

        // Movimento inicial em direção ao inimigo alvo
        phase = 0;
        currentDirection = (targetEnemy.position - transform.position).normalized;
        UpdateSpriteDirection(); // Atualiza a direção do sprite inicialmente
    }

    void Update()
    {
        // Destroi o projétil se sair dos limites
        if (transform.position.x < projectileBounds.min.x || transform.position.x > projectileBounds.max.x ||
            transform.position.y < projectileBounds.min.y || transform.position.y > projectileBounds.max.y)
        {
            Destroy(gameObject);
            return;
        }

        if (phase == 0)
        {
            // Move em direção ao inimigo alvo
            if (targetEnemy != null)
            {
                Vector2 direction = (targetEnemy.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, targetEnemy.position, speed * Time.deltaTime);

                UpdateSpriteDirection(direction.x);

                // Causa dano a todos os inimigos próximos enquanto se move em direção ao alvo
                DamageEnemiesInPath();

                // Checa se o projétil atingiu o alvo específico
                if (Vector2.Distance(transform.position, targetEnemy.position) < 0.1f)
                {
                    Damageable enemy = targetEnemy.GetComponent<Damageable>();
                    if (enemy != null)
                    {
                        enemy.ReceiveDamage(damage, true);
                    }

                    // Inicia a fase 1: mover em direção aleatória
                    phase = 1;
                    phaseTimer = 0f;
                    currentDirection = Random.insideUnitCircle.normalized;
                }
            }
            else
            {
                phase = 1;
                phaseTimer = 0f;
                currentDirection = Random.insideUnitCircle.normalized;
            }
        }
        else if (phase == 1 || phase == 2)
        {
            // Move na direção aleatória atual e aplica dano aos inimigos no caminho
            transform.position += (Vector3)(currentDirection * speed * Time.deltaTime);
            UpdateSpriteDirection(currentDirection.x);
            DamageEnemiesInPath();

            // Controla o tempo de cada fase aleatória
            phaseTimer += Time.deltaTime;
            if (phaseTimer >= 2f)
            {
                if (phase == 1)
                {
                    // Inicia a fase 2 com uma nova direção aleatória
                    phase = 2;
                    phaseTimer = 0f;
                    currentDirection = Random.insideUnitCircle.normalized;
                }
                else
                {
                    // Destroi o projétil após a fase 2
                    Destroy(gameObject);
                }
            }
        }
    }

    void DamageEnemiesInPath()
    {
        // Encontra todos os inimigos com a tag "Enemy" na cena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            Damageable damageableEnemy = enemyObj.GetComponent<Damageable>();
            if (damageableEnemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
                if (distance <= hitRadius)
                {
                    // Aplica dano ao inimigo
                    damageableEnemy.ReceiveDamage(damage, true);
                }
            }
        }
    }

    void UpdateSpriteDirection(float directionX = 0f)
    {
        // Gira o sprite no eixo Y para enfrentar a direção da movimentação
        if (directionX < 0)
        {
            // Move para a esquerda
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (directionX > 0)
        {
            // Move para a direita
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
