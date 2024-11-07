using UnityEngine;

public class XPPickup : MonoBehaviour
{
    public int xpAmount = 5; // Amount of XP this pickup gives
    public float pickupRadius = 3.0f; // Adjust as needed

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
        xpAmount = (int)player.getXPRequired();
    }

    private void Update()
    {
        if (playerTransform != null && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= pickupRadius)
            {
                // Add XP to the player
                player.AddExperience(xpAmount);
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
