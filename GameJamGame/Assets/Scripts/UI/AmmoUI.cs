using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadText;
    [SerializeField] private WeaponShooter weaponShooter;

    private void Start()
    {
        reloadText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (weaponShooter == null || weaponShooter.CurrentWeapon == null)
        {
            ammoText.text = "";
            reloadText.gameObject.SetActive(false);
            return;
        }

        // If reloading, show reload message and update mag/reserve
        if (weaponShooter.IsReloading)
        {
            reloadText.gameObject.SetActive(true);
            ammoText.text = $"Reloading... {weaponShooter.CurrentAmmo} / {weaponShooter.CurrentReserveAmmo}";
        }
        else
        {
            reloadText.gameObject.SetActive(false);

            // Show out-of-ammo message if mag + reserve = 0
            if (weaponShooter.CurrentAmmo == 0 && weaponShooter.CurrentReserveAmmo == 0)
            {
                ammoText.text = "No Ammo";
            }
            else
            {
                ammoText.text = $"{weaponShooter.CurrentAmmo} / {weaponShooter.CurrentReserveAmmo}";
            }
        }
    }
}
