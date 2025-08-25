using System.Collections;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    public Weapon currentWeapon;
    private float lastFireTime;
    private int currentAmmo;
    private bool isReloading = false;
    private Rigidbody2D rb;

    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;
    public Weapon CurrentWeapon => currentWeapon;


    public void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentAmmo = newWeapon.magazineSize;
        isReloading = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (currentWeapon != null)
        {
            currentAmmo = currentWeapon.magazineSize;
        }
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < currentWeapon.magazineSize)
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
            else
            {
                StartCoroutine(Reload());
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            UseAbilityandDropWeapon();
        }
    }

    private void FixedUpdate()
    {
        RotateToMouse();
    }

    private void Shoot()
    {
        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            float spreadOffset = currentWeapon.spreadAngle * (i - (currentWeapon.bulletsPerShot - 1) / 2f);
            Quaternion spreadRot = Quaternion.Euler(0, 0, spreadOffset);

            Vector2 shootDir = spreadRot * transform.right;

            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg);
            bullet.SetActive(true);

            bullet.GetComponent<Bullet>().Fire(shootDir, currentWeapon.bulletSpeed);

            rb.AddForce(-shootDir.normalized * currentWeapon.recoilForce, ForceMode2D.Impulse);
        }
    }

    private void UseAbilityandDropWeapon()
    {
        if (currentWeapon == null) return;

        // TODO: Implement actual ability logic based on currentWeapon.abilityType
        Debug.Log("Used ability: " + currentWeapon.abilityType);

        currentWeapon = null;
        currentAmmo = 0;
        isReloading = false;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentAmmo = currentWeapon.magazineSize;
        isReloading = false;
        Debug.Log("Reloaded " + currentWeapon.weaponName);
    }

    private void RotateToMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
