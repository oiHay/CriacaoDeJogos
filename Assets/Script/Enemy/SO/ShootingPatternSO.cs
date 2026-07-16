using UnityEngine;

[CreateAssetMenu(fileName = "ShootingPatternSO", menuName = "Scriptable Objects/ShootingPatternSO")]
public class ShootingPatternSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab;

    [Header("Audio")] 
    public AudioClip[] shootSounds;

    [Header("Default values")]
    public int projectileDamage;
    public float projectileSpeed;
    public int projectileCount; // Quantos projéteis são instanciados por vez
    public float offsetX; // Quão distante entre si projéteis spawnados ao mesmo tempo estão
    public float shootingInterval; // Tempo entre um tiro e outro em sequência
    public float shootingReload; //Tempo entre cada rajada completa de tiros

    public AudioClip GetRandomShootSound()
    {
        if (shootSounds == null || shootSounds.Length == 0) return null;

        return shootSounds[Random.Range(0, shootSounds.Length)];
    }
}
