using UnityEngine;

public abstract class BulletPattern : ScriptableObject
{
    public abstract void Shoot(Transform firePoint, GameObject bulletPrefab, Transform target);
    
}
