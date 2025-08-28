using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Stats")]
    public Sprite weaponSprite;
    public string weaponName = "New Weapon";
    public float fireRate = 0.2f;
    public float bulletSpeed = 15f;
    public int bulletsPerShot = 1;
    public float spreadAngle = 0f;
    public float recoilForce = 5f;
    public int damage = 1;
    public Vector2 handOffset;

    [Header("Magazine Settings")]
    public int magazineSize = 10;
    public float reloadTime = 1.5f;
    public int maxAmmoReserve = 30;

    [Header("Special Ability")]
    public AbilityType abilityType = AbilityType.None;

    public enum AbilityType
    {
        None,
        BulletStorm,
        BulletBarrage,
        TeleportShot,
        BulletShield
    }

    [Header("References")]
    public GameObject bulletPrefab;
    public AudioClip shootSound;
}
