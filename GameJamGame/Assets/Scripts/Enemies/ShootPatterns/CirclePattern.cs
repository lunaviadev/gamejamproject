using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Patterns/Circle")]
public class CirclePattern : BulletPattern
{
    public int bulletCount = 12;

    public override void Shoot(Transform enemy, GameObject bulletPrefab, Transform player)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, enemy.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Fire(dir, 5f, "Enemy");
        }
    }
}
