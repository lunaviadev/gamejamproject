using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    public GameObject bulletPrefab;

    private Transform player;
    private int currentHealth;
    private float lastShotTime;

    private Vector2 targetPosition;
    private float stoppingDistance = 0.2f;

    private enum EnemyState { Moving, Attacking }
    private EnemyState currentState;
    private float stateTimer;

    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        PickNewTargetPosition();
        currentState = EnemyState.Moving;
    }

    private void Update()
    {
        if (!player) return;

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
            if (enemyData.bulletPattern != null)
            {
                enemyData.bulletPattern.Shoot(transform, bulletPrefab, player);
            }
            else
            {
                ShootAtPlayer();
            }

            lastShotTime = Time.time;
        }

        if (stateTimer <= 0f)
        {
            PickNewTargetPosition();
            currentState = EnemyState.Moving;
        }
    }

    private void PickNewTargetPosition()
    {
    int attempts = 0;
    Vector2 potentialPosition = Vector2.zero;
    bool validPosition = false;

    while (!validPosition && attempts < 20)
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(enemyData.minMoveRadius, enemyData.maxMoveRadius);
        potentialPosition = (Vector2)player.position + randomOffset;

        Collider2D hit = Physics2D.OverlapCircle(potentialPosition, 0.2f, enemyData.obstacleLayer);
        if (hit == null)
            validPosition = true;

        attempts++;
    }

    targetPosition = potentialPosition;
    }


    private void ShootAtPlayer()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Vector2 dir = (player.position - transform.position).normalized;
        newBullet.GetComponent<Bullet>().Fire(dir, enemyData.bulletSpeed, "Enemy");
        newBullet.GetComponent<Bullet>().damage = enemyData.bulletDamage;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
