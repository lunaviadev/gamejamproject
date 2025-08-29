using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;
    public GameObject bulletPrefab;
    [SerializeField] private GameObject[] impactPrefabs;
    [SerializeField] private float OffsetRadius = 0.5f;

    [Header("Flash Settings")]
    [SerializeField] private int flashFlickers = 5;
    [SerializeField] private float flashDuration = 0.1f;

    private Transform player;
    private int currentHealth;
    private float lastShotTime;

    private Vector2 targetPosition;
    private float stoppingDistance = 0.2f;
    private Vector2 currentPosition;

    private enum EnemyState { Moving, Attacking }
    private EnemyState currentState;
    private float stateTimer;

    private Rigidbody2D rb;

    private float collisionTimer;
    private bool isColliding;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        PickNewTargetPosition();
        currentState = EnemyState.Moving;
    }

    private void Update()
    {
        currentPosition = transform.position;

        if (!player) return;

        if (isColliding)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= 2f)
            {
                ForceShootAndMove();
                collisionTimer = 0f;
            }
        }
        else
        {
            collisionTimer = 0f;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= enemyData.detectionRange)
        {
            switch (currentState)
            {
                case EnemyState.Moving:
                    HandleMovement();
                    break;
                case EnemyState.Attacking:
                    HandleAttacking();
                    break;
            }
        }
    }

    private void HandleMovement()
    {
        Vector2 dir = (targetPosition - (Vector2)transform.position);
        float distanceToTarget = dir.magnitude;
        dir.Normalize();

        if (distanceToTarget > stoppingDistance)
        {
            Vector2 move = dir * enemyData.moveSpeed * Time.deltaTime;
            if (move.magnitude > distanceToTarget)
                move = dir * distanceToTarget;

            rb.MovePosition(rb.position + move);
        }
        else
        {
            currentState = EnemyState.Attacking;
            stateTimer = enemyData.attackDuration;
        }
    }

    private void HandleAttacking()
    {
        stateTimer -= Time.deltaTime;

        if (Time.time >= lastShotTime + enemyData.fireRate)
        {
            ShootPatternOrFallback();
            lastShotTime = Time.time;
        }

        if (stateTimer <= 0f)
        {
            PickNewTargetPosition();
            currentState = EnemyState.Moving;
        }
    }

    private void ForceShootAndMove()
    {
        ShootPatternOrFallback();
        lastShotTime = Time.time;
        PickNewTargetPosition();
        currentState = EnemyState.Moving;
    }

    private void ShootPatternOrFallback()
    {
        if (enemyData.bulletPatterns != null && enemyData.bulletPatterns.Length > 0)
        {
            BulletPattern pattern = enemyData.bulletPatterns[Random.Range(0, enemyData.bulletPatterns.Length)];
            pattern.Shoot(transform, bulletPrefab, player); // Assumes pattern handles speed/damage/scale
        }
        else
        {
            ShootAtPlayer(); // Basic fallback
        }
    }

    private void ShootAtPlayer()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 dir = (player.position - transform.position).normalized;

        Bullet bullet = newBullet.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.damage = enemyData.bulletDamage;
            bullet.Fire(dir, enemyData.bulletSpeed, "Enemy");
            newBullet.transform.localScale = Vector3.one * 3f; // Scale up fallback bullet
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (spriteRenderer != null)
            StartCoroutine(FlashFlicker());

        if (impactPrefabs.Length > 0)
            SpawnImpactEffect(currentPosition);

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashFlicker()
    {
        for (int i = 0; i < flashFlickers; i++)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void SpawnImpactEffect(Vector3 spawnPos)
    {
        GameObject prefab = impactPrefabs[Random.Range(0, impactPrefabs.Length)];
        GameObject word = Instantiate(prefab, spawnPos, Quaternion.identity);

        word.transform.position += (Vector3)Random.insideUnitCircle * OffsetRadius;
        float randomRot = Random.Range(0f, 360f);
        word.transform.rotation = Quaternion.Euler(0f, 0f, randomRot);
        float randomScale = Random.Range(0.8f, 1.3f);
        word.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void PickNewTargetPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized *
            Random.Range(enemyData.minMoveRadius, enemyData.maxMoveRadius);

        targetPosition = (Vector2)player.position + randomOffset;

        // Optional: prevent invalid paths (like inside walls)
        if (Physics2D.Raycast(player.position, randomOffset, randomOffset.magnitude, enemyData.obstacleLayer))
        {
            targetPosition = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        collisionTimer = 0f;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
        collisionTimer = 0f;
    }
}
