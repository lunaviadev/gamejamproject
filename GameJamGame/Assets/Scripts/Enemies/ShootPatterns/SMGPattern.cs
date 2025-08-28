using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Patterns/SMG")]
public class SMGPattern : BulletPattern
{
    public int bulletsPerBurst = 7;
    public float spreadAngle = 10f;
    public float bulletSpeed = 6f;

    public override void Shoot(Transform enemy, GameObject bulletPrefab, Transform player)
    {
        Vector2 dirToPlayer = (player.position - enemy.position).normalized;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float angle = baseAngle + angleOffset;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, enemy.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Fire(dir.normalized, bulletSpeed, "Enemy");
        }
    }
}
