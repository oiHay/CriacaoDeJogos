using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    
    private int _currentHealth;
    private bool _isInvunerable;

    private void Start() => _currentHealth = maxHealth;

    public void SetInvulnerable(bool value) => _isInvunerable = true;
    
    public void TakeDamage(int damage)
    {
        if(_isInvunerable) return;
        
        _currentHealth -= damage;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
