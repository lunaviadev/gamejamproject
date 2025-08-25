using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WeaponShooter shooter = collision.GetComponent<WeaponShooter>();
        if (shooter != null && shooter.CurrentWeapon == null)
        {
            shooter.EquipWeapon(weaponData);
            Destroy(gameObject);
        }
    }
}

