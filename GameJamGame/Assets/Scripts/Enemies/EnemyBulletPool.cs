using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance { get; private set; }

    [SerializeField] private GameObject EnemyBulletPrefab;
    [SerializeField] private int poolSize = 50;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(EnemyBulletPrefab, transform);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetEnemyBullet()
    {
        foreach (var bullet in pool)
        {
            if (!bullet.activeInHierarchy)
                return bullet;
        }

        GameObject newBullet = Instantiate(EnemyBulletPrefab, transform);
        newBullet.SetActive(false);
        pool.Add(newBullet);
        return newBullet;
    }

    
}
