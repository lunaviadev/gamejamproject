using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Patterns/Spiral")]
public class SpiralPattern : BulletPattern
{
    public float angleIncrement = 10f;
    public int bulletsPerShot = 1;
    private float currentAngle = 0f;

    public override void Shoot(Transform enemy, GameObject bulletPrefab, Transform player)
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = currentAngle;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, enemy.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Fire(dir, 5f, "Enemy");

            currentAngle += angleIncrement;
        }
    }
}
