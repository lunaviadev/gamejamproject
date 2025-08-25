using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float lifeTime = 2f;
    private float spawnTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    public void Fire(Vector2 direction, float speed)
    {
        rb.velocity = direction.normalized * speed;
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
        // TODO: damage enemies, particles, etc.
        // For now, just deactivate the bullet
        gameObject.SetActive(false);
    }
}
