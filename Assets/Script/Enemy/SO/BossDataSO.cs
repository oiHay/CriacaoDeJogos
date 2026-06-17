using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BossDataSO")]
public class BossDataSO : ScriptableObject
{
    [Header("Health")] public float maxHealth;

    [Header("Basic Attack - Fan Shot")] 
    public GameObject fanProjectilePrefab;
    public float basicChargeTime;
    public int fanProjectileCount;
    [Range(0f, 360f)] public float fanAngle;
    public float fanProjectileSpeed;

    [Header("Intermediate Attack - Laser")]
    public GameObject circleIndicatorPrefab;
    public GameObject laserPrefab;
    public float intermediateChargeTime;
    public float circleChaseTime;
    public float circleChaseSpeed;
    public float laserDuration;
    public int laserDamage;

    [Header("Hard Attack - Bombs")] 
    public GameObject bombIndicatorPrefab;
    public GameObject bombExplosionPrefab;
    public float hardChargeTime;
    public int bombCount;
    public float bombWarningTime;
    public int bombDamage;
    public Vector2 spawnAreaX;
    public Vector2 spawnAreaZ;

    [Header("Phase Aggression")] 
    [Range(0.1f, 1f)] public float phase2ReloadMultiplier = 0.75f;
    [Range(0.1f, 1f)] public float phase3ReloadMultiplier = 0.5f;

    [Header("Cycle")] 
    public float baseReloadTime;
    public float timeBetweenAttacks;
}
