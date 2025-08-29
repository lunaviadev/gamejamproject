using UnityEngine;

public abstract class BulletPattern : ScriptableObject
{
    // Original method (still works if you don't need extras)
    public abstract void Shoot(Transform firePoint, GameObject bulletPrefab, Transform target);

    // Optional override for scaling, damage, speed
    public virtual void ShootWithScale(Transform firePoint, GameObject bulletPrefab, Transform target, float scale, int damage, float speed)
    {
        // Default fallback just calls the original
        Shoot(firePoint, bulletPrefab, target);
    }
}
