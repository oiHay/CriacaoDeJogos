using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDataSO", menuName = "Scriptable Objects/ProjectileDataSO")]
public class ProjectileDataSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab;
    
    [Header("Default values")]
    public AnimationCurve speedCurve;
}
