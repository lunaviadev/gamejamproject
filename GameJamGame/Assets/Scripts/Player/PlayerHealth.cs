using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 6;
    private int currentHealth;

    public static event Action<int, int> OnHealthChanged;

    [Header("Damage Settings")]
    public List<GameObject> damagingPrefabs = new List<GameObject>();

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player took damage. Current HP: " + currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damagingPrefabs.Contains(collision.gameObject))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (damagingPrefabs.Contains(other.gameObject))
        {
            TakeDamage(1);
        }
    }
}
