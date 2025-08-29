using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Stats")]
    public string enemyName;
    public int maxHealth = 5;
    public float moveSpeed = 2f;
    public float fireRate = 1f;
    public float bulletSpeed = 10f;
    public int bulletDamage = 1;

    [Header("Behavior")]
    public float detectionRange = 10f;
    public BulletPattern[] bulletPatterns; 

    [Header("AI Behaviour")]
    public float attackDuration = 2f;
    public float minMoveRadius = 2f;
    public float maxMoveRadius = 5f;
    public LayerMask obstacleLayer;

    [Header("Boss-Specific")]
    public float bulletScale = 1f; 
    public float patternSwitchTime = 3f;
}
