using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    
    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("EnemyProjectile")) return;
        
        _playerHealth.TakeDamage(1);
        Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Enemy")) return;
        
        _playerHealth.TakeDamage(1);
    }
}
