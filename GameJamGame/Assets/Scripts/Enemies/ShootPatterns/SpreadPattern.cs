using UnityEngine;

[CreateAssetMenu(fileName = "SpreadPattern", menuName = "Enemies/Patterns/Spread")]
public class SpreadPattern : BulletPattern
{
    public int bulletCount = 5;
    public float spreadAngle = 45f;

    public override void Shoot(Transform firePoint, GameObject bulletPrefab, Transform target)
    {
        float startAngle = -spreadAngle / 2;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Vector2 dir = rot * (target.position - firePoint.position).normalized;

            GameObject bullet = Object.Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Fire(dir, 6f);
        }
    }
}
