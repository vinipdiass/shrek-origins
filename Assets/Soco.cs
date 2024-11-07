using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soco : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    public float damage; // Valor do dano
    public float hitRange = 0.5f; // Distância para considerar um hit

    private Bounds projectileBounds;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        float projectileMargin = 0.5f;
        projectileBounds = new Bounds();
        projectileBounds.SetMinMax(
            new Vector3(Globals.WorldBounds.min.x - projectileMargin, Globals.WorldBounds.min.y - projectileMargin, 0.0f),
            new Vector3(Globals.WorldBounds.max.x + projectileMargin, Globals.WorldBounds.max.y + projectileMargin, 0.0f)
        );
    }

    void Update()
    {
        // Checa se o projétil está fora dos limites
        if (transform.position.x < projectileBounds.min.x || transform.position.x > projectileBounds.max.x ||
            transform.position.y < projectileBounds.min.y || transform.position.y > projectileBounds.max.y)
        {
            Destroy(gameObject);
            return;
        }

        // Checa a colisão com inimigos
        CheckForEnemyHit();
    }

    void CheckForEnemyHit()
    {
        // Encontra todos os inimigos com a tag "Enemy" na cena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            // Checa se o inimigo implementa a interface IDamageable
            Damageable damageableEnemy = enemyObj.GetComponent<Damageable>();
            if (damageableEnemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
                if (distance <= hitRange)
                {
                    // Aplica dano ao inimigo
                    damageableEnemy.ReceiveDamage(damage, true);
                    break; // Sai do loop já que o projétil foi destruído
                }
            }
        }
    }
}
