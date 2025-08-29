using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 6;
    private int currentHealth;
    public bool Damaged = false;
    public Animator animator;
    public float InvincibleDuration = 0.4f;

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
        Damaged = true;
        StartCoroutine(Invincible());
        currentHealth -= dmg;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player took damage. Current HP: " + currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator Invincible()
    {


        if (Damaged == true)
        {
            animator.SetBool("Damaged", true);
        }

        float Ielapsed = 0f;

        gameObject.layer = LayerMask.NameToLayer("Invincible");

        while (Ielapsed < InvincibleDuration)
        {
            Ielapsed += Time.deltaTime;
            yield return null;
        }

        Damaged = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        animator.SetBool("Damaged", false);
        
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
