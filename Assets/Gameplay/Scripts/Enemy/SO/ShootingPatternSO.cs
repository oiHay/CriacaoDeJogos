using UnityEngine;

[CreateAssetMenu(fileName = "ShootingPatternSO", menuName = "Scriptable Objects/ShootingPatternSO")]
public class ShootingPatternSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab;

    [Header("Default values")]
    public int projectileDamage;
    public int projectileSpeedBase;
    public int projectileCount; // Quantos projéteis são instanciados por vez
    public float offsetX; // Quão distante entre si projéteis spawnados ao mesmo tempo estão
    public float shootingInterval; // Tempo entre um tiro e outro em sequência
    public float shootingReload; //Tempo entre cada rajada completa de tiros
    public AnimationCurve projectileSpeedPerRound;
}
