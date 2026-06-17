using System;
using UnityEngine;

public class BossCollision : MonoBehaviour
{
    private BossBehaviour _boss;

    private void Awake()
    {
        _boss = GetComponent<BossBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Projectile")) return;

        ProjectileCollision projectile = other.GetComponent<ProjectileCollision>();

        if (projectile != null)
            _boss.TakeDamage(projectile.Damage);
        
        Destroy(other.gameObject);
    }
}
