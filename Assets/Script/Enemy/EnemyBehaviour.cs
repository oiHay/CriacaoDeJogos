using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private ParticleSystem hitParticle;

    [Header("Audio")] 
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private AudioClip[] hitSounds;
    
    private EnemyDataSO _enemyData;
    private GameStatesEventSO _gameStateEvent;
    private int _roundIndex;
    private float _currentHealth;
    private bool _isGameActive;

    private ShootingPatternSO _patternOverride;

    public Action OnEnemyDestroyed;
    public Action OnShot;

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

    public void SetPattern(ShootingPatternSO newPattern) => _patternOverride = newPattern;

    private IEnumerator ShootingLoop()
    {
        ShootingPatternSO pattern = _patternOverride ?? _enemyData.shootingPattern;

        while (_isGameActive)
        {
            yield return new WaitForSeconds(pattern.shootingReload);

            for (int i = 0; i < pattern.projectileCount; i++)
            {
                SpawnProjectile(i, pattern);

                if (pattern.projectileCount > 1)
                    yield return new WaitForSeconds(pattern.shootingInterval);
            }
        }
    }

    private void SpawnProjectile(int index, ShootingPatternSO pattern)
    {
        OnShot?.Invoke();

        float offset = 0f;
        if (pattern.projectileCount > 1)
            offset = (index - (pattern.projectileCount - 1) / 2.0f) * pattern.offsetX;

        Vector3 spawnPos = transform.position + new Vector3(offset, 0, 0);

        GameObject projectile = Instantiate(
            pattern.projectilePrefab,
            spawnPos,
            pattern.projectilePrefab.transform.rotation
        );
        
        AudioManager.PlaySound(pattern.GetRandomShootSound());

        projectile.GetComponent<ProjectileBehaviour>().SetDirection(Vector3.back);
        projectile.GetComponent<ProjectileBehaviour>().Initialize(_gameStateEvent, pattern.projectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Projectile")) return;

        ProjectileCollision projectile = other.GetComponent<ProjectileCollision>();
        
        if (projectile != null)
            TakeDamage(projectile.Damage);
        
        Destroy(other.gameObject);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            CallParticle();
            PlayRandomDeathSound();
            OnEnemyDestroyed?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Instantiate(hitParticle, transform.position + Vector3.back * 0.5f, hitParticle.transform.rotation);
            PlayRandomHitSound();
        }
    }
    
    private void CallParticle()
    {
        if(particlePrefab == null) return;
        
        GameObject spawnedParticleObj =  Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

        ParticleSystem ps = spawnedParticleObj.GetComponent<ParticleSystem>();

        if (ps != null)
            ps.Play();
    }
    
    private void PlayRandomDeathSound()
    {
        if (deathSounds == null || deathSounds.Length == 0) return;

        int index = Random.Range(0, deathSounds.Length);
        AudioManager.PlaySound(deathSounds[index]);
    }

    private void PlayRandomHitSound()
    {
        if (hitSounds == null || hitSounds.Length == 0) return;

        int index = Random.Range(0, hitSounds.Length);
        AudioManager.PlaySound(hitSounds[index]);
    }
}
