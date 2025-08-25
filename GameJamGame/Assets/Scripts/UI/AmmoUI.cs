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

        // If reloading, show reload message
        if (weaponShooter.IsReloading)
        {
            reloadText.gameObject.SetActive(true);
            ammoText.text = $"0 / {weaponShooter.CurrentWeapon.magazineSize}";
        }
        else
        {
            reloadText.gameObject.SetActive(false);
            ammoText.text = $"{weaponShooter.CurrentAmmo} / {weaponShooter.CurrentWeapon.magazineSize}";
        }
    }
}
