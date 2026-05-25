using System;
using System.Collections;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour
{
    [Header("References")]
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

        if (_isGameActive)
            StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnProjectile()
    {
        while (_isGameActive)
        {
            yield return new WaitForSeconds(pattern.shootingReload);

            for (int i = 0; i < pattern.projectileCount; i++)
            {
                SpawnSingleProjectile(i);

                if (pattern.projectileCount > 1)
                    yield return new WaitForSeconds(pattern.shootingInterval);
            }
        }
    }

    private void SpawnSingleProjectile(int index)
    {
        float offset = 0f;

        if (pattern.projectileCount > 1)
            offset = (index - (pattern.projectileCount - 1) / 2.0f) * pattern.offsetX;

        Vector3 spawnPos = transform.position + new Vector3(offset, 0, projectileSpawnOffsetZ);

        GameObject projectile = Instantiate(
            pattern.projectilePrefab,
            spawnPos,
            Quaternion.Euler(90, 0, 0)
        );

        float speed = pattern.projectileSpeedPerRound.Evaluate(0);
        
        projectile.GetComponent<ProjectileBehaviour>().Initialize(_gameStateEvent, speed);
        projectile.GetComponent<ProjectileCollision>().Initialize(pattern.projectileDamage);
    }
}
