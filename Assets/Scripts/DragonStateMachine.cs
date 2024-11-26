using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStateMachine : MonoBehaviour, Damageable
{
    [Header("Attributes")]
    public float totalHealth;
    public float currentHealth;
    public float attackDamage;
    public float moveSpeed;
    public float colliderRadius;
    public float enemyVision;
    public GameObject fireballPrefab; // O projétil da bola de fogo
    public float fireballSpeed = 5f;
    public float fireballCooldown = 4f; // Tempo para lançar o projétil
    public GameObject damageTextPrefab;

    [Header("Components")]
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    [Header("Others")]
    private EnemyState currentState;
    private EnemyState lastState;
    private Transform playerTransform;
    private float distanceToPlayer;

    // Cooldown for fireball attack
    private float lastFireballTime;

    // Cooldown for receiving damage
    private float receiveDamageCooldown = 0.2f;
    private float lastDamageReceivedTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeState(EnemyState.ChasingTarget);
    }

    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (playerTransform != null)
        {
            distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        }

        switch (currentState)
        {
            case EnemyState.ChasingTarget:
                HandleChasingTargetState();
                break;
            case EnemyState.RecoveringFromHit:
                HandleRecoveringFromHit();
                break;
            case EnemyState.Dead:
                HandleDeadState();
                break;
        }

        // Verifica se já está na hora de lançar uma bola de fogo
        if (Time.time >= lastFireballTime + fireballCooldown)
        {
            LaunchFireball();
        }
    }

    public void ChangeState(EnemyState newState)
    {
        lastState = currentState;
        currentState = newState;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// States
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void HandleChasingTargetState()
    {
        if (playerTransform == null)
        {
            return;
        }

        // Direção em direção ao player
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Move o dragão em direção ao jogador
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

        // Olha na direção do jogador
        LookTarget(direction);

        // Dano ao jogador ao colidir com o dragão
        if (Vector2.Distance(transform.position, playerTransform.position) <= colliderRadius)
        {
            // Aplica dano ao jogador e knockback
            PlayerStateMachine player = playerTransform.GetComponent<PlayerStateMachine>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                ApplyKnockback(player);
            }
        }
    }

    void HandleRecoveringFromHit()
    {
        // Logic for recovering after being hit (if needed)
    }

    void HandleDeadState()
    {
        // Handle death (this can be similar to the old man's death behavior)
        Destroy(gameObject);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// Coroutines
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Função para lançar uma bola de fogo
    private void LaunchFireball()
    {
        // Cria a bola de fogo
        if (fireballPrefab != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            // Calcula a direção em que a bola de fogo irá
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

            // Dá um impulso à bola de fogo
            if (rb != null)
            {
                rb.velocity = direction * fireballSpeed;
            }

            // Atualiza o tempo do último lançamento de fogo
            lastFireballTime = Time.time;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// Damage and Knockback
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void ReceiveDamage(float damage, bool knockback)
    {
        // Implementar o dano (semelhante ao que foi feito no OldManStateMachine)
        if (Time.time >= lastDamageReceivedTime + receiveDamageCooldown && currentState != EnemyState.Dead)
        {
            currentHealth -= damage;
            lastDamageReceivedTime = Time.time;

            // Mostra o texto de dano
            ShowDamageText(damage);

            // Se a saúde chegar a zero, muda para o estado de morte
            if (currentHealth <= 0)
            {
                ChangeState(EnemyState.Dead);
            }
        }
    }

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject dmgText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            DamageText dmgTextScript = dmgText.GetComponent<DamageText>();
            if (dmgTextScript != null)
            {
                dmgTextScript.SetText(damage.ToString());
            }
        }
    }

    private void ApplyKnockback(PlayerStateMachine player)
    {
        // Aplica knockback no jogador
        Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// Others
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void LookTarget(Vector2 direction)
    {
        // Rotaciona o dragão para olhar na direção do player
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
