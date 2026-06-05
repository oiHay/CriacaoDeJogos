using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private float blinkInterval = 0.1f;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    
    private int _currentHealth;
    private bool _isInvulnerable;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _currentHealth = maxHealth;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

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
            StartCoroutine(InvincibilityRoutine());
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
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
