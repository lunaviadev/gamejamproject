using UnityEngine;

[CreateAssetMenu(fileName = "StraightPattern", menuName = "Enemies/Patterns/Straight")]
public class StraightPattern : BulletPattern
{
    public override void Shoot(Transform firePoint, GameObject bulletPrefab, Transform target)
    {
        Vector2 dir = (target.position - firePoint.position).normalized;
        GameObject bullet = Object.Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Fire(dir, 8f);
    }
}
