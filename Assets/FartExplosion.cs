using System.Collections;
using UnityEngine;

public class FartExplosion : MonoBehaviour
{
    public float damage = 50f;          // Damage dealt by the explosion
    public float explosionRadius = 2f;  // Radius of the explosion
    public float explosionDuration = 0.5f; // Duration before the explosion disappears

    void Start()
    {
        // Damage enemies within the explosion radius
        DamageEnemiesInRadius();

        // Destroy the explosion object after a short duration
        Destroy(gameObject, explosionDuration);
    }

    private void DamageEnemiesInRadius()
    {
        // Find all objects with the tag "Enemy" within the radius
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            // Check if the enemy has the Damageable interface and is within range
            Damageable damageableEnemy = enemyObj.GetComponent<Damageable>();
            if (damageableEnemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
                if (distance <= explosionRadius)
                {
                    // Apply damage to the enemy
                    damageableEnemy.ReceiveDamage(damage, true);
                }
            }
        }
    }

    // Optional: Visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
