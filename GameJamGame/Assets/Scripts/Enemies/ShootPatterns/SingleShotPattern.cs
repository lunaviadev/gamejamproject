using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Patterns/SingleShot")]
public class SingleShotPattern : BulletPattern
{
    public float bulletSpeed = 5f;

    public override void Shoot(Transform enemy, GameObject bulletPrefab, Transform player)
    {
        // Get direction from enemy to player
        Vector2 direction = (player.position - enemy.position).normalized;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, enemy.position, Quaternion.identity);

        // Fire the bullet straight at the player
        bullet.GetComponent<Bullet>().Fire(direction, bulletSpeed, "Enemy");
    }
}
