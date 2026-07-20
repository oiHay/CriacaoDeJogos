using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBehaviour : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if (debugMode)
            Debug.Log(message);
    }

    #endregion

    [Header("Data")] 
    [SerializeField] private BossDataSO bossData;
    [SerializeField] private GameStatesEventSO gameStateEvent;

    [Header("VFX")] 
    [SerializeField] private GameObject deathParticlePrefab;
    [SerializeField] private ParticleSystem hitParticle;

    [Header("Audio - Health")] 
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip[] hitSounds;
    
    [Header("Audio - Attacks")] 
    [SerializeField] private AudioSource chargeSource; 
    [SerializeField] private AudioClip fanChargeSound;
    [SerializeField] private AudioClip[] fanShootSounds;
    [SerializeField] private AudioClip laserChargeSound;
    [SerializeField] private AudioClip laserFireSound;
    [SerializeField] private AudioClip bombChargeSound;
    [SerializeField] private AudioClip explosionSound;

    public event Action OnBossDestroyed;
    public event Action<BossPhase> OnPhaseChanged;
    public event Action<float, float> OnHealthChanged;

    private float _currentHealth;
    private BossPhase _currentPhase = BossPhase.Phase1;
    private Transform _playerTransform;

    private void Awake()
    {
        _currentHealth = bossData.maxHealth;
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void OnEnable() => gameStateEvent.OnRaised += HandleStateChanged;
    private void OnDisable() => gameStateEvent.OnRaised -= HandleStateChanged;

    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                StopAllCoroutines();
                StartCoroutine(AttackLoop());
                break;
            
            case GameState.Paused:
            case GameState.Victory:
            case GameState.GameOver:
                StopAllCoroutines();
                StopCharge();
                break;
        }
    }

    #region AttackLoop

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return StartCoroutine(ExecutePhaseAttacks());
            yield return new WaitForSeconds(GetCurrentReload());
        }
    }

    private IEnumerator ExecutePhaseAttacks()
    {
        yield return StartCoroutine(BasicAttack());

        if (_currentPhase >= BossPhase.Phase2)
        {
            yield return new WaitForSeconds(bossData.timeBetweenAttacks);
            yield return StartCoroutine(IntermediateAttack());
        }

        if (_currentPhase == BossPhase.Phase3)
        {
            yield return new WaitForSeconds(bossData.timeBetweenAttacks);
            yield return StartCoroutine(HardAttack());
        }
    }

    private float GetCurrentReload()
    {
        return _currentPhase switch
        {
            BossPhase.Phase1 => bossData.baseReloadTime,
            BossPhase.Phase2 => bossData.baseReloadTime * bossData.phase2ReloadMultiplier,
            BossPhase.Phase3 => bossData.baseReloadTime * bossData.phase3ReloadMultiplier,
            _                => bossData.baseReloadTime
        };
    }
    
    #endregion

    #region Attacks

    #region Basic - Fan

    private IEnumerator BasicAttack()
    {
        PlayCharge(fanChargeSound);
        
        yield return new WaitForSeconds(bossData.baseReloadTime);

        StopCharge();
        PlayRandomFanShootSound();
        
        for (int i = 0; i < bossData.fanProjectileCount; i++)
        {
            float t = bossData.fanProjectileCount == 1
                ? 0.5f
                : (float)i / (bossData.fanProjectileCount - 1);

            float angle = Mathf.Lerp(-bossData.fanAngle / 2f, bossData.fanAngle / 2f, t);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.back;

            SpawnFanProjectile(direction);
        }
    }

    private void SpawnFanProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(
            bossData.fanProjectilePrefab,
            transform.position,
            Quaternion.LookRotation(direction)
        );

        ProjectileBehaviour pb = projectile.GetComponent<ProjectileBehaviour>();
        pb.SetDirection(direction);
        pb.Initialize(gameStateEvent, bossData.fanProjectileSpeed);
    }

    #endregion

    #region Intermediate - Laser

    private IEnumerator IntermediateAttack()
    {
        PlayCharge(laserChargeSound);
        
        yield return new WaitForSeconds(bossData.intermediateChargeTime);
        
        if (_playerTransform == null) yield break;

        Vector3 startPos = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);
        GameObject circle = Instantiate(bossData.circleIndicatorPrefab, startPos, Quaternion.identity);

        float elapsed = 0f;
        while (elapsed < bossData.circleChaseTime)
        {
            if (circle == null) yield break;

            Vector3 target = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);
            circle.transform.position = Vector3.Lerp(circle.transform.position, target,
                Time.deltaTime * bossData.circleChaseSpeed);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (circle != null)
        {
            StopCharge();
            AudioManager.PlaySound(laserFireSound);
            SpawnLaser(transform.position, circle.transform.position);
            Destroy(circle);
        }
    }
    
    private void SpawnLaser(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        float distance = dir.magnitude;

        GameObject laser = Instantiate(
            bossData.laserPrefab,
            (from + to) / 2f,
            Quaternion.FromToRotation(Vector3.up, dir.normalized)
        );

        Vector3 scale = laser.transform.localScale;
        scale.y = distance / 2f;
        laser.transform.localScale = scale;
    }

    #endregion

    #region Hard - Bomb

    private IEnumerator HardAttack()
    {
        PlayCharge(bombChargeSound);
        
        yield return new WaitForSeconds(bossData.hardChargeTime);

        List<GameObject> indicators = new();

        for (int i = 0; i < bossData.bombCount; i++)
        {
            float x = UnityEngine.Random.Range(bossData.spawnAreaX.x, bossData.spawnAreaX.y);
            float z = UnityEngine.Random.Range(bossData.spawnAreaZ.x, bossData.spawnAreaZ.y);

            indicators.Add(Instantiate(
                bossData.bombIndicatorPrefab,
                new Vector3(x, 0f, z),
                Quaternion.identity
            ));
        }

        yield return new WaitForSeconds(bossData.bombWarningTime);

        StopCharge();
        AudioManager.PlaySound(explosionSound);
        
        foreach (var indicator in indicators)
        {
            if (indicator == null) continue;
            Instantiate(bossData.bombExplosionPrefab, indicator.transform.position, Quaternion.identity);
            Destroy(indicator);
        }
    }

    #endregion

    #endregion

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0f);
        OnHealthChanged?.Invoke(_currentHealth, bossData.maxHealth);
        PlayRandomHitSound();
        CheckPhaseTransition();

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        if (hitParticle != null)
            Instantiate(hitParticle, transform.position + Vector3.back * 0.5f, hitParticle.transform.rotation);
    }
    
    private void CheckPhaseTransition()
    {
        BossPhase newPhase = _currentHealth > bossData.maxHealth * (2f / 3f) ? BossPhase.Phase1 :
                             _currentHealth > bossData.maxHealth * (1f / 3f) ? BossPhase.Phase2 :
                             BossPhase.Phase3;

        if (newPhase == _currentPhase) return;

        _currentPhase = newPhase;
        OnPhaseChanged?.Invoke(_currentPhase);
        DebugMessage("Boss phase changed to: " + _currentPhase);
    }

    private void Die()
    {
        if (deathParticlePrefab != null)
        {
            GameObject fx = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

            var p = fx.GetComponentInChildren<ParticleSystem>();
            if (p !=null)
                p.Play();

            AudioManager.Instance.PlaySfx(deathSound);
        }

        OnBossDestroyed?.Invoke();
        
        GameManager.Instance.ChangeState(GameState.Victory);
        Destroy(gameObject);
    }

    private void PlayRandomHitSound()
    {
        if (hitSounds == null || hitSounds.Length == 0) return;

        int index = Random.Range(0, hitSounds.Length);
        AudioManager.PlaySound(hitSounds[index]);
    }
    
    private void PlayRandomFanShootSound()
    {
        if (fanShootSounds == null || fanShootSounds.Length == 0) return;

        int index = Random.Range(0, fanShootSounds.Length);
        AudioManager.PlaySound(fanShootSounds[index]);
    }

    private void PlayCharge(AudioClip clip)
    {
        if (chargeSource == null || clip == null) return;

        chargeSource.clip = clip;
        chargeSource.Play();
    }

    private void StopCharge()
    {
        if (chargeSource!=null)
            chargeSource.Stop();
    }
}
