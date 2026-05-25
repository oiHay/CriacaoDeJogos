using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    public event Action OnDeath;
    private int _currentHealth;

    private void Start() => _currentHealth = maxHealth;

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        // Destroy(gameObject);
    }
}
