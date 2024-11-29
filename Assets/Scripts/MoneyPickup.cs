using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    public int moneyAmount; // Amount of XP this pickup gives
    public float pickupRadius = 1.0f; // Adjust as needed

    private Transform playerTransform;
    private PlayerStateMachine player;

    private void Start()
    {
        // Find the player in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            player = playerObj.GetComponent<PlayerStateMachine>();
        }

        moneyAmount = Random.Range(5, 41);
    }

    private void Update()
    {
        if (playerTransform != null && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= pickupRadius)
            {
                // Add XP to the player
                GameDataManager.instance.AdicionarDinheiro(moneyAmount);
                // Destroy the XP pickup
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the pickup radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
