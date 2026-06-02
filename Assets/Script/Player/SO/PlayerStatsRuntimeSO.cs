using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsRuntimeSO", menuName = "Scriptable Objects/PlayerStatsRuntimeSO")]
public class PlayerStatsRuntimeSO : ScriptableObject
{
    public bool hasDash;
    public float explosionChance;
    public List<ShootingPatternSO> extraPatterns = new List<ShootingPatternSO>();

    public void Reset()
    {
        hasDash = false;
        explosionChance = 0f;
        extraPatterns.Clear();
    }

    public void Apply(PowerUpSO powerUp)
    {
        switch (powerUp.effectType)
        {
            case PowerUpEffectType.Dash: 
                hasDash = true; 
                break;
            
            case PowerUpEffectType.BulletExplosion:
                explosionChance = Mathf.Clamp01(explosionChance + powerUp.value);
                break;
            case PowerUpEffectType.ProjectileCount:
                extraPatterns.Add(powerUp.extraPattern);
                break;
        }
    }
    
}
