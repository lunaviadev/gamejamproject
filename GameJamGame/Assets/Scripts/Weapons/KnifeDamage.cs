using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
