using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject enemyPrefab;
    
    [Header("Default values")]
    public float health;
    public int projectileCount;

    [Header("Additional values per round")]
    public AnimationCurve healthPerRound;

    [Header("Score values")]
    public int scoreValue;
}
