using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public int damage;
    private float lifeTime = 2f;
    private float spawnTime;
    public string shooterTag;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
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

        // Despawn bullet
        gameObject.SetActive(false);
    }
}
