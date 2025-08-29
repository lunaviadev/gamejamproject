using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public int damage;
    private float lifeTime = 2f;
    private float spawnTime;
    public string shooterTag;

    public bool isTeleportBullet = false;
    public System.Action<Vector2> OnBulletHit;

    [Header("Bullet Scaling")]
    public bool autoScale = false;           // Enable per bullet
    public float scaleMultiplier = 1f;       // Scale to apply when enabled

    private Vector3 originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
        isTeleportBullet = false;
        OnBulletHit = null;

        // Reset scale in case reused from pool
        transform.localScale = originalScale;

        // Apply scale if enabled
        if (autoScale && scaleMultiplier != 1f)
        {
            transform.localScale = originalScale * scaleMultiplier;
        }
    }

    public void Fire(Vector2 direction, float speed, string tag = "Player")
    {
        rb.linearVelocity = direction.normalized * speed;
        shooterTag = tag;
    }

    private void Update()
    {
        if (Time.time > spawnTime + lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(shooterTag)) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Player hit for " + damage + " damage.");
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy hit for " + damage + " damage.");
            }
        }

        if (isTeleportBullet)
        {
            OnBulletHit?.Invoke(transform.position);
        }

        gameObject.SetActive(false);
    }
}
