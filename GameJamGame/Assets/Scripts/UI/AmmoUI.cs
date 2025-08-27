using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class AmmoUI : MonoBehaviour
{
    [Header("Ammo UI")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private WeaponShooter weaponShooter;

    [Header("Health UI")]
    [SerializeField] private List<Image> heartImages;

    private void Start()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthUI;
    }

    private void Update()
    {
        if (weaponShooter == null || weaponShooter.CurrentWeapon == null)
        {
            ammoText.text = "";
            return;
        }

        if (weaponShooter.IsReloading)
        {
            ammoText.text = $"Reloading... {weaponShooter.CurrentAmmo} / {weaponShooter.CurrentReserveAmmo}";
        }


            if (weaponShooter.CurrentAmmo == 0 && weaponShooter.CurrentReserveAmmo == 0)
                ammoText.text = "No Ammo";
            else
                ammoText.text = $"{weaponShooter.CurrentAmmo} / {weaponShooter.CurrentReserveAmmo}";
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
                heartImages[i].enabled = true;
            else
                heartImages[i].enabled = false;
        }
    }
}
