using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Stats")]
    public string weaponName = "New Weapon";
    public float fireRate = 0.2f; // Time between shots
    public float bulletSpeed = 15f;
    public int bulletsPerShot = 1; // for shotguns or burst fire
    public float spreadAngle = 0f; // degrees between bullets
    public float recoilForce = 5f; // force applied to player on shoot. FORCE PER BULLET


    [Header("Magazine Settings")]
    public int magazineSize = 10; // bullets per magazine
    public float reloadTime = 1.5f; // time to reload

    [Header("Special Ability")]
    public AbilityType abilityType = AbilityType.None;

     public enum AbilityType
    {
        None,
        ExplosiveShot,
        GroundSlam,
        PiercingShot,
        TeleportShot,
        Shield
    }

    [Header("References")]
    public GameObject bulletPrefab;
}
