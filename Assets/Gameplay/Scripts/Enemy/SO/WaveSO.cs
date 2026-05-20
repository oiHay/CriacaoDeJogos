using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO", menuName = "Scriptable Objects/WaveSO")]
public class WaveSO : ScriptableObject
{
    public SpawnEntry[] spawnEntries;
}

[Serializable]
public class SpawnEntry
{
    public EnemyDataSO enemyData;
    public int quantity;
}
