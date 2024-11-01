using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldWomanStateMachine : MonoBehaviour, Damageable
{
    [Header("Attributes")]
    public float totalHealth;
    public float currentHealth;
    public float attackDamage;
    public float moveSpeed;
    public float colliderRadius;
    public float enemyVision;
    public float damageRange = 0.5f;
    public GameObject damageTextPrefab;
    public GameObject xpPickupPrefab;

    [Header("Components")]
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb;

    [Header("Others")]
    private EnemyState currentState;
    private EnemyState lastState;
    private Transform playerTransform;
    private float distanceToPlayer;
    public float playerDistanceToStop;

    [Header("Coroutines")]
    private Coroutine recoveringFromHitCoroutine;

    [Header("Control Booleans")]
    private bool isRecoveringFromHit;

    // Cooldown variables for attack
    private float damageCooldown = 1.0f;
    private float lastDamageTime;

    // Cooldown for receiving damage
    private float receiveDamageCooldown = 0.2f;
    private float lastDamageReceivedTime;

    // Static list to keep track of all enemies
    private static List<OldWomanStateMachine> allEnemies = new List<OldWomanStateMachine>();

    void OnEnable()
    {
        allEnemies.Add(this);
    }

    void OnDisable()
    {
        allEnemies.Remove(this);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

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
            case EnemyState.Idle:
                HandleIdleState();
                break;
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

        // Verifies if within player range to cause damage
        CheckAndCauseDamageToPlayer();
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    public void ChangeState(EnemyState newState)
    {
        lastState = currentState;
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                animator.SetInteger("State", 0);
                break;
            case EnemyState.ChasingTarget:
                animator.SetInteger("State", 1);
                break;
            case EnemyState.RecoveringFromHit:
                animator.SetInteger("State", 3);
                break;
            case EnemyState.Dead:
                animator.SetInteger("State", 0);
                break;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// States
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void HandleIdleState()
    {
        ChangeState(EnemyState.ChasingTarget);
    }

    void HandleChasingTargetState()
    {
        if (playerTransform == null)
        {
            return;
        }

        // Direction towards the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Compute separation from other enemies
        Vector2 separation = ComputeSeparation();

        // Weight factors
        float separationStrength = 1.5f; // Adjust this value as needed
        float playerAttractionStrength = 1.0f;

        // Combine the direction towards the player and the separation
        Vector2 finalDirection = (direction * playerAttractionStrength + separation * separationStrength).normalized;

        // Look at the target
        LookTarget(finalDirection);

        // Move the enemy
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + finalDirection, moveSpeed * Time.deltaTime);

        // Check if within attack range
        if (distanceToPlayer <= playerDistanceToStop)
        {
            CheckAndCauseDamageToPlayer();
        }
    }

    private Vector2 ComputeSeparation()
    {
        Vector2 separation = Vector2.zero;
        float separationRadius = 0.5f; // Adjust this value as needed

        foreach (var enemy in allEnemies)
        {
            if (enemy != this)
            {
                Vector2 toEnemy = transform.position - enemy.transform.position;
                float distance = toEnemy.magnitude;
                if (distance < separationRadius && distance > 0.01f)
                {
                    // The closer the enemy, the stronger the repulsion
                    separation += toEnemy.normalized / distance;
                }
            }
        }

        return separation;
    }

    void HandleRecoveringFromHit()
    {
        if (recoveringFromHitCoroutine == null) recoveringFromHitCoroutine = StartCoroutine(RecoveringFromHitCoroutine());
    }

    void HandleDeadState()
    {
        if (xpPickupPrefab != null)
        {
            Instantiate(xpPickupPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// Coroutines
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator RecoveringFromHitCoroutine()
    {
        isRecoveringFromHit = true;
        animator.SetTrigger("Take Damage");
        yield return new WaitForSeconds(1.0f);

        ChangeState(EnemyState.ChasingTarget);

        isRecoveringFromHit = false;
        recoveringFromHitCoroutine = null;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// Others
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StopAllCoroutinesMy()
    {
        if (recoveringFromHitCoroutine != null)
        {
            StopCoroutine(recoveringFromHitCoroutine);
            isRecoveringFromHit = false;
            recoveringFromHitCoroutine = null;
        }
    }

    public void ReceiveDamage(float damage)
    {
        // Check cooldown for receiving damage
        if (Time.time >= lastDamageReceivedTime + receiveDamageCooldown && currentState != EnemyState.Dead)
        {
            currentHealth -= damage;
            lastDamageReceivedTime = Time.time;

            // Show damage text
            ShowDamageText(damage);

            StopAllCoroutinesMy();
            if (currentHealth <= 0) ChangeState(EnemyState.Dead);
        }
    }

    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            // Instantiate damage text at the position of OldMan
            GameObject dmgText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);

            // Set the text to show received damage
            DamageText dmgTextScript = dmgText.GetComponent<DamageText>();
            if (dmgTextScript != null)
            {
                dmgTextScript.SetText(damage.ToString());
            }
        }
    }

    private void CheckAndCauseDamageToPlayer()
    {
        // Check distance with the additional damage range (colliderRadius + damageRange)
        if (distanceToPlayer <= colliderRadius + damageRange && Time.time >= lastDamageTime + damageCooldown)
        {
            CauseDamageOnPlayer();
            lastDamageTime = Time.time;
        }
    }

    void CauseDamageOnPlayer()
    {
        PlayerStateMachine player = playerTransform.GetComponent<PlayerStateMachine>();
        if (player != null)
        {
            player.TakeDamage(attackDamage);
        }
    }

    void LookTarget(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, enemyVision);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, playerDistanceToStop);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.position + transform.right * (colliderRadius + damageRange)), colliderRadius + damageRange);
    }
}
