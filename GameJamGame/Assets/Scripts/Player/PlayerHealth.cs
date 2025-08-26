using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 6;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("Player took damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // Restart level or trigger game over
    }
}
