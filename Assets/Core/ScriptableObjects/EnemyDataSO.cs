using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject enemyPrefab;
    public ShootingPatternSO shootingPattern;
    
    [Header("Default values")]
    public float health;

    [Header("Additional values per round")]
    public AnimationCurve healthPerRound;

    [Header("Score values")]
    public int scoreValue;
}
