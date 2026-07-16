using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private ParticleSystem hitParticle;

    [Header("Health Values")]
    public int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("Audio")] 
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private AudioClip hitSound;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    
    private int _currentHealth;
    private bool _isInvulnerable;
    private MeshRenderer _meshRenderer;

    private bool _indestructible;
    private float _regenDelay = 1.5f;
    
    private void Start()
    {
        _currentHealth = maxHealth;
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void SetIndestructible(bool value) => _indestructible = value;

    public void SetInvulnerable(bool value) => _isInvulnerable = value;
    
    public void TakeDamage(int damage)
    {
        if(_isInvulnerable) return;
        
        _currentHealth -= damage;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Instantiate(hitParticle, transform.position + Vector3.forward * 0.5f, hitParticle.transform.rotation);
            cameraShake?.Shake();
            AudioManager.PlaySound(hitSound);
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void HealToFull() => StartCoroutine(HealAfterDelay());

    private IEnumerator HealAfterDelay()
    {
        yield return new WaitForSeconds(_regenDelay);
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }
    
    private IEnumerator InvincibilityRoutine()
    {
        _isInvulnerable = true;

        float timer = invincibilityDuration;
        while (timer > 0f)
        {
            _meshRenderer.enabled = !_meshRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer -= blinkInterval;
        }

        _meshRenderer.enabled = true;
        _isInvulnerable = false;
    }

    private void Die()
    {
        if (_indestructible)
        {
            HealToFull();
            return;
        }
        
        CallParticle();
        PlayRandomDeathSound();
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private void CallParticle()
    {
        if (particlePrefab == null) return;

        GameObject spawnedParticleObjs =
            Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

        ParticleSystem ps = spawnedParticleObjs.GetComponent<ParticleSystem>();
        
        if (ps != null)
            ps.Play();

    }

    private void PlayRandomDeathSound()
    {
        if (deathSounds == null || deathSounds.Length == 0) return;

        int index = Random.Range(0, deathSounds.Length);
        AudioManager.PlaySound(deathSounds[index]);
    }
}
