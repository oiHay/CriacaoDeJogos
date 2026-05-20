using UnityEngine;

[CreateAssetMenu(fileName = "RoundSO", menuName = "Scriptable Objects/RoundSO")]
public class RoundSO : ScriptableObject
{
    public WaveSO[] waves;
    public FormationLayoutSO formationLayout;
    public float formationSpeed;
}
