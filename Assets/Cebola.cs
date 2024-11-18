using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cebola : MonoBehaviour
{
    public float speed;
    public float damage;
    public Transform targetEnemy;
    private Bounds projectileBounds;

    public GameObject gasAreaPrefab; // Prefab of the gas area to instantiate upon impact

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
    }

    void Update()
    {
        // Destroy the projectile if it goes out of bounds
        if (transform.position.x < projectileBounds.min.x || transform.position.x > projectileBounds.max.x ||
            transform.position.y < projectileBounds.min.y || transform.position.y > projectileBounds.max.y)
        {
            Destroy(gameObject);
            return;
        }


        if (targetEnemy != null)
        {
            // Move the projectile toward the enemy's current position
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.position, speed * Time.deltaTime);

            // Rotate the projectile to face the enemy
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Check if the projectile has reached the enemy
            if (Vector2.Distance(transform.position, targetEnemy.position) < 0.1f)
            {
                Damageable enemy = targetEnemy.GetComponent<Damageable>();
                if (enemy != null)
                {
                    //enemy.ReceiveDamage(damage);
                }

                // Instantiate the gas area at the impact position
                Instantiate(gasAreaPrefab, transform.position, Quaternion.identity);

                // Destroy the projectile
                Destroy(gameObject);
            }
        }
        else
        {
            // If the enemy no longer exists, destroy the projectile
            Destroy(gameObject);
        }
    }
}
