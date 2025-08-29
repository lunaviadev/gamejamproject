using System.Collections;
using UnityEngine;

public class BossPatternController : MonoBehaviour
{
    public EnemyData bossData;
    public GameObject bulletPrefab;
    public Transform player;

    private float fireCooldown;
    private BulletPattern currentPattern;

    private void Start()
    {
        if (bossData.bulletPatterns.Length == 0)
        {
            Debug.LogError("No bullet patterns assigned to EnemyData.");
            return;
        }

        StartCoroutine(SwitchPatternsRoutine());
    }

    private void Update()
    {
        if (player == null || bossData == null || currentPattern == null)
            return;

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            fireCooldown = bossData.fireRate;

            // Fire with scale
            FireWithScaling(currentPattern);
        }
    }

    private void FireWithScaling(BulletPattern pattern)
    {
        pattern.ShootWithScale(transform, bulletPrefab, player, bossData.bulletScale, bossData.bulletDamage, bossData.bulletSpeed);
    }

    private IEnumerator SwitchPatternsRoutine()
    {
        while (true)
        {
            currentPattern = bossData.bulletPatterns[Random.Range(0, bossData.bulletPatterns.Length)];
            yield return new WaitForSeconds(bossData.patternSwitchTime);
        }
    }
}
