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

    [Header("Weapon UI")]
    [SerializeField] private List<Image> weaponIcons;
    private Dictionary<string, Image> weaponIconDict = new Dictionary<string, Image>();
    private Image currentActiveIcon;

    private void Start()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthUI;

        foreach (var icon in weaponIcons)
        {
            if (icon != null)
            {
                weaponIconDict[icon.name] = icon; 
                icon.enabled = false;
            }
        }
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

            if(currentActiveIcon != null)
            {
                currentActiveIcon.enabled = false;
                currentActiveIcon = null;
            }

            return;


        }

        if (weaponShooter.IsReloading)
        {
            ammoText.text = $"Reloading... {weaponShooter.CurrentAmmo} / {weaponShooter.CurrentReserveAmmo}";
        }
        else if (weaponShooter.CurrentAmmo == 0 && weaponShooter.CurrentReserveAmmo == 0)
        {
            ammoText.text = "No Ammo";
        }
        else
        {
            ammoText.text = $"{weaponShooter.CurrentAmmo} / {weaponShooter.CurrentReserveAmmo}";
        }

        string weaponName = weaponShooter.CurrentWeapon.weaponName;

        if (currentActiveIcon != null)
            currentActiveIcon.enabled = false;

        if (weaponIconDict.TryGetValue(weaponName, out Image newIcon))
        {
            newIcon.enabled = true;
            currentActiveIcon = newIcon;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].enabled = i < currentHealth;
        }
    }
}
