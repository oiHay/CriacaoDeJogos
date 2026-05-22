using System;
using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemyDataSO _enemyData;
    private GameStatesEventSO _gameStateEvent;
    private int _roundIndex;
    private float _currentHealth;
    private bool _isGameActive;

    public Action OnEnemyDestroyed;

    public void Initialize(EnemyDataSO enemyData, int roundIndex, GameStatesEventSO gameStatesEvent)
    {
        _enemyData = enemyData;
        _roundIndex = roundIndex;
        _gameStateEvent = gameStatesEvent;
        _currentHealth = _enemyData.health + _enemyData.healthPerRound.Evaluate(_roundIndex);
    }

    public void SetGameState(GameState state)
    {
        _isGameActive = state == GameState.Playing;

        if (!_isGameActive)
            StopAllCoroutines();
        else
            StartCoroutine(ShootingLoop());
    }

    private IEnumerator ShootingLoop()
    {
        ShootingPatternSO pattern = _enemyData.shootingPattern;

        while (_isGameActive)
        {
            yield return new WaitForSeconds(pattern.shootingReload);

            for (int i = 0; i < pattern.projectileCount; i++)
            {
                SpawnProjectile(i);

                if (pattern.projectileCount > 1)
                    yield return new WaitForSeconds(pattern.shootingInterval);
            }
        }
    }

    private void SpawnProjectile(int index)
    {
        ShootingPatternSO pattern = _enemyData.shootingPattern;

        float offset = 0f;

        if (pattern.projectileCount > 1)
            offset = (index - (pattern.projectileCount - 1) / 2.0f) * pattern.offsetX;

        Vector3 spawnPos = transform.position + new Vector3(offset, 0, 0);

        GameObject projectile = Instantiate(
            pattern.projectilePrefab,
            spawnPos,
            pattern.projectilePrefab.transform.rotation
        );

        ProjectileBehaviour projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.SetDirection(Vector3.back);
        projectile.GetComponent<ProjectileBehaviour>().Initialize(_gameStateEvent, pattern.projectileSpeedPerRound.Evaluate(_roundIndex));
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnEnemyDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(_currentHealth);
    }
}
