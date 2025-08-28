using System.Collections;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject[] wordPrefabs;
    [SerializeField] private float wordOffsetRadius = 0.5f;

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

        if (wordPrefabs.Length > 0)
        {
            SpawnWordEffect(weaponTransform.position);
        }

    }


    private void UseAbilityandDropWeapon()
    {
        if (currentWeapon == null) return;

        Debug.Log("Used ability: " + currentWeapon.abilityType);

        switch (currentWeapon.abilityType)
        {
            case Weapon.AbilityType.BulletShield:
                ActivateBulletShield();
                break;

        }

        currentWeapon = null;
        currentAmmo = 0;
        currentReserveAmmo = 0;
        isReloading = false;

        if (weaponHolder != null)
        {
            weaponHolder.UnequipWeapon();
        }
    }

private void SpawnWordEffect(Vector3 spawnPos)
{

    Debug.Log("Spawning word effect!");
    Debug.Log("Spawning word at: " + spawnPos);
    GameObject prefab = wordPrefabs[Random.Range(0, wordPrefabs.Length)];
    GameObject word = Instantiate(prefab, spawnPos, Quaternion.identity);

    word.transform.position += (Vector3)Random.insideUnitCircle * wordOffsetRadius;

    float randomRot = Random.Range(0f, 360f);
    word.transform.rotation = Quaternion.Euler(0f, 0f, randomRot);

    float randomScale = Random.Range(0.8f, 1.3f);
    word.transform.localScale = new Vector3(randomScale, randomScale, 1f);
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


    //ABILITIES

private void ActivateBulletShield()
{
    int abilityDamage = currentWeapon != null ? currentWeapon.damage : 1; // fallback if null
    StartCoroutine(BulletShieldRoutine(abilityDamage));
}

private IEnumerator BulletShieldRoutine(int abilityDamage)
{
    int bulletCount = 16;
    float bulletSpeed = 15f;
    float radius = 0.5f;
    float delayBetweenRings = 0.3f;
    int ringCount = 3;

    for (int ring = 0; ring < ringCount; ring++)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject newBullet = BulletPool.Instance.GetBullet();
            if (newBullet != null)
            {
                newBullet.transform.position = (Vector2)transform.position + dir * radius;
                newBullet.transform.rotation = Quaternion.identity;
                newBullet.SetActive(true);

                Bullet bulletComp = newBullet.GetComponent<Bullet>();
                if (bulletComp != null)
                {
                    bulletComp.Fire(dir, bulletSpeed, "Player");
                    bulletComp.damage = abilityDamage;
                }
                else
                {
                    Debug.LogWarning("Bullet prefab missing Bullet component!");
                }
            }
        }

        if (ring < ringCount - 1)
            yield return new WaitForSeconds(delayBetweenRings);
    }
}



}
