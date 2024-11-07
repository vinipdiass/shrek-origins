using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rugido : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    public float damage;
    public float hitRange = 0.5f;

    private Bounds projectileBounds;
    private Transform targetEnemy;

    public bool shouldTargetEnemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (shouldTargetEnemy)
        {
            targetEnemy = FindClosestEnemy();

            if (targetEnemy != null)
            {
                Vector2 direction = (targetEnemy.position - transform.position).normalized;
                rb.velocity = direction * speed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                rb.velocity = transform.right * speed;
            }
        }
        else
        {
            rb.velocity = transform.right * speed;
        }

        float projectileMargin = 0.5f;
        projectileBounds = new Bounds();
        projectileBounds.SetMinMax(
            new Vector3(Globals.WorldBounds.min.x - projectileMargin, Globals.WorldBounds.min.y - projectileMargin, 0.0f),
            new Vector3(Globals.WorldBounds.max.x + projectileMargin, Globals.WorldBounds.max.y + projectileMargin, 0.0f)
        );
    }

    void Update()
    {
        if (transform.position.x < projectileBounds.min.x || transform.position.x > projectileBounds.max.x ||
            transform.position.y < projectileBounds.min.y || transform.position.y > projectileBounds.max.y)
        {
            Destroy(gameObject);
            return;
        }

        CheckForEnemyHit();
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemyObj in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyObj.transform;
            }
        }
        return closestEnemy;
    }

    void CheckForEnemyHit()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            Damageable enemy = enemyObj.GetComponent<Damageable>();
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
                if (distance <= hitRange)
                {
                    enemy.ReceiveDamage(damage, true);
                    break;
                }
            }
        }
    }
}
