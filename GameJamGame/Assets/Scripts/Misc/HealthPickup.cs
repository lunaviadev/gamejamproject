using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(-healthAmount);
            Destroy(gameObject);
        }
    }
}
