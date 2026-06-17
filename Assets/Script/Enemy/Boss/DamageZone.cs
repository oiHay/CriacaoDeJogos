using System;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float duration = 0.5f;

    private void Start()
    {
        Destroy(gameObject,duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        health?.TakeDamage(damage);
    }
}
