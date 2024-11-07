using UnityEngine;

public class HPPickup : MonoBehaviour
{
    public int xpAmount = 5; // Amount of XP this pickup gives
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
    }

    private void Update()
    {
        if (playerTransform != null && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= pickupRadius)
            {
                player.AddHp(40);
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
