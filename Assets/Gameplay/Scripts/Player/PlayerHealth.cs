using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }
}
