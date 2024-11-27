using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Attributes")]
    public float damage = 40f;             // Dano da bola de fogo
    public float knockbackForce = 5f;      // Força do knockback
    public float lifetime = 5f;            // Tempo de vida da bola de fogo (para destruição após um tempo)
    public float damageRange = 0.5f;       // Distância de dano ao jogador (similar ao damageRange do OldMan)
    public float speed = 10f;              // Velocidade da bola de fogo

    private float timeAlive;
    private Transform playerTransform;
    private Rigidbody2D rb;

    void Start()
    {
        // Inicializa o Rigidbody2D e o Transform do jogador
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Checa se o Rigidbody2D está presente
        if (rb == null)
        {
            Debug.LogError("Fireball requires a Rigidbody2D component.");
        }

        // Se o jogador foi encontrado, calcula a direção e rotaciona a bola de fogo
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            RotateFireball(direction); // Rotaciona a bola de fogo para a direção do jogador
            MoveFireball(direction);   // Move a bola de fogo em direção ao jogador
        }
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > lifetime)
        {
            Destroy(gameObject);  // Destrói a bola de fogo após o tempo de vida
        }

        // Verifica se o jogador está dentro do alcance de dano
        CheckAndCauseDamageToPlayer();
    }

    // Verifica se o jogador está dentro do alcance da bola de fogo e causa o dano
    private void CheckAndCauseDamageToPlayer()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // Verifica se o jogador está dentro do alcance de dano
            if (distanceToPlayer <= damageRange)
            {
                CauseDamageOnPlayer();
            }
        }
    }

    // Aplica o dano e o knockback no jogador
    private void CauseDamageOnPlayer()
    {
        PlayerStateMachine player = playerTransform.GetComponent<PlayerStateMachine>();
        if (player != null)
        {
            player.TakeDamage(damage);  // Aplica o dano no jogador

            // Aplica o knockback (movimento para longe da bola de fogo)
            Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;
        }

        Destroy(gameObject);  // Destrói a bola de fogo após causar dano
    }

    // Rotaciona a bola de fogo para a direção onde o jogador está
    private void RotateFireball(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Move a bola de fogo na direção do jogador
    private void MoveFireball(Vector2 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction * speed;  // Move a bola de fogo
        }
    }

    // Método opcional para visualizar a área de colisão no Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);  // Área de dano
    }
}
