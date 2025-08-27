using System.Collections;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    
    [SerializeField] private CameraFollow cameraFollow; 
    
    public Weapon currentWeapon;
    public WeaponHolder weaponHolder;
    private float lastFireTime;
    private int currentAmmo;
    private int currentReserveAmmo;
    private bool isReloading = false;
    private Rigidbody2D rb;

    public int CurrentAmmo => currentAmmo;
    public int CurrentReserveAmmo => currentReserveAmmo;
    public bool IsReloading => isReloading;
    public Weapon CurrentWeapon => currentWeapon;

    public void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentAmmo = newWeapon.magazineSize;
        currentReserveAmmo = newWeapon.maxAmmoReserve;
        isReloading = false;

        if (weaponHolder != null)
        {
            weaponHolder.EquipWeapon(newWeapon.weaponName);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (currentWeapon != null)
        {
            currentAmmo = currentWeapon.magazineSize;
            currentReserveAmmo = currentWeapon.maxAmmoReserve;
        }
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        if (Input.GetButtonDown("Fire2"))
        {
            UseAbilityandDropWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < currentWeapon.magazineSize && currentReserveAmmo > 0) // <-- check reserve
        {
            StartCoroutine(Reload());
            return;
        }
        if (isReloading) return;

        

        if (Input.GetButton("Fire1") && Time.time >= lastFireTime + currentWeapon.fireRate)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                lastFireTime = Time.time;
                currentAmmo--;
            }
            else if (currentReserveAmmo > 0)
            {
                StartCoroutine(Reload());
            }
            else
            {
                Debug.Log("Out of ammo for " + currentWeapon.weaponName);
            }
        }
    }


    private void Shoot()
    {
        if (weaponHolder == null || weaponHolder.activeWeaponSprite == null) return;

        Transform weaponTransform = weaponHolder.activeWeaponSprite.spriteRenderer.transform;

        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            float spreadOffset = currentWeapon.spreadAngle * (i - (currentWeapon.bulletsPerShot - 1) / 2f);
            Quaternion spreadRot = Quaternion.Euler(0, 0, spreadOffset);

            Vector2 shootDir = spreadRot * weaponTransform.up;

            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = weaponTransform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg);
            bullet.SetActive(true);

            bullet.GetComponent<Bullet>().Fire(shootDir, currentWeapon.bulletSpeed, "Player");
            bullet.GetComponent<Bullet>().damage = currentWeapon.damage;
            rb.AddForce(-shootDir.normalized * currentWeapon.recoilForce, ForceMode2D.Impulse);
        }
        
        if (cameraFollow != null)
        {

            cameraFollow.Shake(0.2f);
        }
    }



    private void UseAbilityandDropWeapon()
    {
        if (currentWeapon == null) return;

        Debug.Log("Used ability: " + currentWeapon.abilityType);

        currentWeapon = null;
        currentAmmo = 0;
        currentReserveAmmo = 0;
        isReloading = false;
        
        if (weaponHolder != null)
        {
            weaponHolder.UnequipWeapon();
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.reloadTime);

        int ammoNeeded = currentWeapon.magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, currentReserveAmmo);

        currentAmmo += ammoToReload;
        currentReserveAmmo -= ammoToReload;

        isReloading = false;
        Debug.Log("Reloaded " + currentWeapon.weaponName);
    }

}
