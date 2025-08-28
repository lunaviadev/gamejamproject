using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleDrops;
    [SerializeField] private Transform spawnPoint;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSpawned) return;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null && player.isRolling)
        {
            SpawnRandomItem();
            return;
        }

        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.shooterTag == "Player")
            {
                SpawnRandomItem();
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void SpawnRandomItem()
    {
        if (possibleDrops.Length == 0) return;

        int rand = Random.Range(0, possibleDrops.Length);
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(possibleDrops[rand], spawnPos, Quaternion.identity);

        hasSpawned = true;
        gameObject.SetActive(false);
    }
}
