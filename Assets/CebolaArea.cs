using System.Collections;
using UnityEngine;

public class GasArea : MonoBehaviour
{
    public float damagePerSecond = 15f;  // Damage per second to enemies within the area
    public float areaRadius = 2f;        // Radius of the gas area
    public float duration = 5f;          // Duration before the gas area disappears
    private float damageInterval = 0.5f; // Interval between damage ticks
    public float baseDamagePerSecond = 15f;


    void Start()
    {
        // Start applying damage over time
        StartCoroutine(ApplyDamageOverTime());

        // Destroy the gas area after its duration
        Destroy(gameObject, duration);

    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            DamageEnemiesInRadius();

            // Wait for the next damage tick
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void DamageEnemiesInRadius()
    {
        // Find all enemies by tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            // Check if the enemy has the Damageable component
            Damageable damageableEnemy = enemyObj.GetComponent<Damageable>();
            if (damageableEnemy != null)
            {
                // Calculate the distance between the gas area and the enemy
                float distance = Vector2.Distance(transform.position, enemyObj.transform.position);
                
                // Apply damage if within the gas area radius
                if (distance <= areaRadius)
                {
                    damageableEnemy.ReceiveDamage(damagePerSecond, false);
                }
            }
        }
    }

    // Optional: Visualize the gas area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
