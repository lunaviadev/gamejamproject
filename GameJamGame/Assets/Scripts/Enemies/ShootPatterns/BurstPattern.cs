using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Patterns/Burst")]
public class BurstPattern : BulletPattern
{
    public int bulletsPerBurst = 5;
    public float spreadAngle = 45f;

    public override void Shoot(Transform enemy, GameObject bulletPrefab, Transform player)
    {
        Vector2 dirToPlayer = (player.position - enemy.position).normalized;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float angle = baseAngle + Mathf.Lerp(-spreadAngle / 2, spreadAngle / 2, i / (float)(bulletsPerBurst - 1));
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, enemy.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Fire(dir, 5f, "Enemy");
        }
    }
}
