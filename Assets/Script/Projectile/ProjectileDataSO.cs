using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDataSO", menuName = "Scriptable Objects/ProjectileDataSO")]
public class ProjectileDataSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab; // Referência ao prefab do projétil
    
    [Header("Default values")]
    public AnimationCurve speedCurve; 
}
