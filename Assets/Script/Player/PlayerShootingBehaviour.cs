using System.Collections;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerStatsRuntimeSO playerStatsSO;
    [SerializeField] private ShootingPatternSO pattern;

    [Header("Gun Parameters")]
    [SerializeField] private float projectileSpawnOffsetZ;

    private bool _isGameActive;
    private GameStatesEventSO _gameStateEvent;

    public void Initialize(GameStatesEventSO eventSO)
    {
        _gameStateEvent = eventSO;
    }

    public void SetGameState(GameState state)
    {
        _isGameActive = state == GameState.Playing;
        StopAllCoroutines();

        if (!_isGameActive) return;
            
        StartCoroutine(SpawnProjectile(pattern));

        foreach (var extra in playerStatsSO.extraPatterns)
        {
            StartCoroutine(SpawnProjectile(extra));
        }
    }

    private IEnumerator SpawnProjectile(ShootingPatternSO p)
    {
        while (_isGameActive)
        {
            yield return new WaitForSeconds(p.shootingReload);

            for (int i = 0; i < p.projectileCount; i++)
            {
                SpawnSingleProjectile(i, p);

                if (p.projectileCount > 1)
                    yield return new WaitForSeconds(p.shootingInterval);
            }
        }
    }

    private void SpawnSingleProjectile(int index, ShootingPatternSO p)
    {
        float offset = 0f;

        if (p.projectileCount > 1)
            offset = (index - (p.projectileCount - 1) / 2.0f) * p.offsetX;

        Vector3 spawnPos = transform.position + new Vector3(offset, 0, projectileSpawnOffsetZ);

        GameObject projectile = Instantiate(
            p.projectilePrefab,
            spawnPos,
            Quaternion.Euler(90, 0, 0)
        );

        float speed = p.projectileSpeedPerRound.Evaluate(0);
        
        projectile.GetComponent<ProjectileBehaviour>().Initialize(_gameStateEvent, speed);
        projectile.GetComponent<ProjectileCollision>().Initialize(p.projectileDamage, playerStatsSO.explosionChance);
    }
}
