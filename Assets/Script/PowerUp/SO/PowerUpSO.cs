using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "Scriptable Objects/PowerUpSO")]
public class PowerUpSO : ScriptableObject
{
    public string powerUpName;
    [TextArea] public string description;
    public Sprite icon;
    public PowerUpEffectType effectType;
    public float value;
    public ShootingPatternSO extraPattern;
}
