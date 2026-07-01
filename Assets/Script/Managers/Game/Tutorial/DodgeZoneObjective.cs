using System;
using UnityEngine;

public class DodgeZoneObjective : TutorialObjective
{
    [SerializeField] private EnemyBehaviour enemy;
    [SerializeField] private int dodgesRequired = 3;
    [SerializeField] private float dodgeCooldown = 1f;

    private int _dodgeCount;
    private bool _active;
    private bool _playerInZone;
    private float _lastDodgeTime = -10f;

    public override void StartObjective()
    {
        _active = true;
        enemy.OnShot += HandleShot;
    }

    private void HandleShot()
    {
        if (!_active) return;
        if (Time.time - _lastDodgeTime < dodgeCooldown) return;
        if (_playerInZone) return;

        _lastDodgeTime = Time.time;
        _dodgeCount++;

        if (_dodgeCount >= dodgesRequired)
        {
            _active = false;
            enemy.OnShot -= HandleShot;
            Completed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInZone = false;
    }
}
