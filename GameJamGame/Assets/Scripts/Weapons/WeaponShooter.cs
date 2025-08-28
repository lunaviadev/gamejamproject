using System.Collections;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("Camera & Effects")]
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject[] wordPrefabs;
    [SerializeField] private float wordOffsetRadius = 0.5f;
    [SerializeField] private GameObject knifeObject;

    [Header("Reload Icon")]
    [SerializeField] private Transform reloadIconTransform; // assign the sprite GameObject
    [SerializeField] private float reloadIconHeight = 1.5f; // offset above player
    [SerializeField] private float reloadIconRotationSpeed = 360f; // degrees/sec

    [Header("Weapon Data")]
    public Weapon currentWeapon;
    public WeaponHolder weaponHolder;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    private float lastFireTime;
    private int currentAmmo;
    private int currentReserveAmmo;
    private bool isReloading = false;
    public bool UsedAbility = false;

    public int CurrentAmmo => currentAmmo;
    public int CurrentReserveAmmo => currentReserveAmmo;
    public bool IsReloading => isReloading;
    public Weapon CurrentWeapon => currentWeapon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();

        if (currentWeapon != null)
        {
            currentAmmo = currentWeapon.magazineSize;
            currentReserveAmmo = currentWeapon.maxAmmoReserve;
            if (knifeObject != null) knifeObject.SetActive(false);
        }
        else
        {
            if (knifeObject != null) knifeObject.SetActive(true);
        }

        if (reloadIconTransform != null)
            reloadIconTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentWeapon == null)
        {
            if (knifeObject != null && !knifeObject.activeSelf)
                knifeObject.SetActive(true);
            return;
        }
        else
        {
            if (knifeObject != null && knifeObject.activeSelf)
                knifeObject.SetActive(false);
        }

        // Update reload icon position and rotation
        if (reloadIconTransform != null)
        {
            reloadIconTransform.position = transform.position + Vector3.up * reloadIconHeight;
            if (isReloading)
                reloadIconTransform.Rotate(0, 0, reloadIconRotationSpeed * Time.deltaTime);
        }

        // Use ability
        if (Input.GetButtonDown("Fire2"))
        {
            UseAbilityandDropWeapon();
        }

        // Start reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < currentWeapon.magazineSize && currentReserveAmmo > 0)
        {
            StartCoroutine(ReloadRoutine());
            return;
        }

        if (isReloading) return;

        // Shoot weapon
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
                StartCoroutine(ReloadRoutine());
            }
            else
            {
                Debug.Log("Out of ammo for " + currentWeapon.weaponName);
            }
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentAmmo = newWeapon.magazineSize;
        currentReserveAmmo = newWeapon.maxAmmoReserve;
        isReloading = false;

        if (weaponHolder != null)
            weaponHolder.EquipWeapon(newWeapon.weaponName);

        if (knifeObject != null)
            knifeObject.SetActive(false);
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

            Bullet bulletComp = bullet.GetComponent<Bullet>();
            if (bulletComp != null)
            {
                bulletComp.Fire(shootDir, currentWeapon.bulletSpeed, "Player");
                bulletComp.damage = currentWeapon.damage;
            }

            rb.AddForce(-shootDir.normalized * currentWeapon.recoilForce, ForceMode2D.Impulse);
        }

        if (cameraFollow != null)
            cameraFollow.Shake(0.2f);

        if (wordPrefabs.Length > 0)
            SpawnWordEffect(weaponTransform.position);

        if (currentWeapon.shootSound != null)
            audioSource.PlayOneShot(currentWeapon.shootSound);
    }

    private void SpawnWordEffect(Vector3 spawnPos)
    {
        GameObject prefab = wordPrefabs[Random.Range(0, wordPrefabs.Length)];
        GameObject word = Instantiate(prefab, spawnPos, Quaternion.identity);

        word.transform.position += (Vector3)Random.insideUnitCircle * wordOffsetRadius;
        word.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        float randomScale = Random.Range(0.8f, 1.3f);
        word.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (reloadIconTransform != null)
            reloadIconTransform.gameObject.SetActive(true);

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        int ammoNeeded = currentWeapon.magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, currentReserveAmmo);

        currentAmmo += ammoToReload;
        currentReserveAmmo -= ammoToReload;

        isReloading = false;

        if (reloadIconTransform != null)
            reloadIconTransform.gameObject.SetActive(false);

        Debug.Log("Reloaded " + currentWeapon.weaponName);
    }

    private void UseAbilityandDropWeapon()
    {
        if (currentWeapon == null) return;

        UsedAbility = true;
        Debug.Log("Used ability: " + currentWeapon.abilityType);

        switch (currentWeapon.abilityType)
        {
            case Weapon.AbilityType.BulletShield:
                StartCoroutine(BulletShieldRoutine(currentWeapon.damage));
                break;
            case Weapon.AbilityType.BulletStorm:
                StartCoroutine(BulletBeamRoutine(currentWeapon.damage));
                break;
            case Weapon.AbilityType.BulletBarrage:
                StartCoroutine(BulletBarrageRoutine(currentWeapon.damage));
                break;
            case Weapon.AbilityType.TeleportShot:
                StartCoroutine(TeleportShotRoutine(currentWeapon.damage));
                break;
        }

        currentWeapon = null;
        currentAmmo = 0;
        currentReserveAmmo = 0;
        isReloading = false;

        if (weaponHolder != null)
            weaponHolder.UnequipWeapon();

        if (knifeObject != null)
            knifeObject.SetActive(true);
    }

    #region ABILITIES

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
                }
            }

            if (ring < ringCount - 1)
                yield return new WaitForSeconds(delayBetweenRings);
        }
    }

    private IEnumerator BulletBeamRoutine(int abilityDamage)
    {
        float beamDuration = 1.5f;
        float fireInterval = 0.02f;
        float bulletSpeed = 25f;
        float elapsed = 0f;

        Transform weaponTransform = weaponHolder.activeWeaponSprite.spriteRenderer.transform;

        while (elapsed < beamDuration)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mouseWorldPos - weaponTransform.position).normalized;

            GameObject newBullet = BulletPool.Instance.GetBullet();
            if (newBullet != null)
            {
                newBullet.transform.position = weaponTransform.position;
                newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                newBullet.SetActive(true);

                Bullet bulletComp = newBullet.GetComponent<Bullet>();
                if (bulletComp != null)
                {
                    bulletComp.Fire(dir, bulletSpeed, "Player");
                    bulletComp.damage = abilityDamage;
                }
            }

            yield return new WaitForSeconds(fireInterval);
            elapsed += fireInterval;
        }
    }

    private IEnumerator BulletBarrageRoutine(int abilityDamage)
    {
        int waves = 5;
        int bulletsPerWave = 12;
        float spreadAngle = 60f;
        float bulletSpeed = 20f;
        float delayBetweenWaves = 0.1f;

        Transform weaponTransform = weaponHolder.activeWeaponSprite.spriteRenderer.transform;

        for (int wave = 0; wave < waves; wave++)
        {
            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angleOffset = -spreadAngle / 2f + (spreadAngle / (bulletsPerWave - 1)) * i;
                Quaternion rot = Quaternion.Euler(0, 0, weaponTransform.eulerAngles.z + angleOffset);
                Vector2 dir = rot * Vector2.up;

                GameObject newBullet = BulletPool.Instance.GetBullet();
                if (newBullet != null)
                {
                    newBullet.transform.position = weaponTransform.position;
                    newBullet.transform.rotation = rot;
                    newBullet.SetActive(true);

                    Bullet bulletComp = newBullet.GetComponent<Bullet>();
                    if (bulletComp != null)
                    {
                        bulletComp.Fire(dir, bulletSpeed, "Player");
                        bulletComp.damage = abilityDamage;
                    }
                }
            }

            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    private IEnumerator TeleportShotRoutine(int abilityDamage)
    {
        float bulletSpeed = 20f;
        Transform weaponTransform = weaponHolder.activeWeaponSprite.spriteRenderer.transform;
        Vector2 shootDir = weaponTransform.up;

        GameObject teleportBullet = BulletPool.Instance.GetBullet();
        if (teleportBullet != null)
        {
            teleportBullet.transform.position = weaponTransform.position;
            teleportBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg);
            teleportBullet.SetActive(true);

            Bullet bulletComp = teleportBullet.GetComponent<Bullet>();
            if (bulletComp != null)
            {
                bulletComp.isTeleportBullet = true;
                bulletComp.Fire(shootDir, bulletSpeed, "Player");
                bulletComp.damage = abilityDamage;

                bulletComp.OnBulletHit = (Vector2 hitPos) =>
                {
                    transform.position = hitPos;
                    teleportBullet.SetActive(false);
                };
            }
        }

        yield return null;
    }

    #endregion
}
