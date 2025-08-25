using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    public GameObject bulletPrefab;

    private Transform player;
    private int currentHealth;
    private float lastShotTime;

    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!player) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= enemyData.detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * enemyData.moveSpeed * Time.deltaTime);

            // Shoot if cooldown passed
            if (Time.time >= lastShotTime + enemyData.fireRate)
            {
                ShootAtPlayer();
                lastShotTime = Time.time;
            }
        }
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
