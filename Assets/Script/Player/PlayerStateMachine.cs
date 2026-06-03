using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private PlayerStatsRuntimeSO playerStats;

    private PlayerController _controller;
    private PlayerShootingBehaviour _shooting;
    private PlayerHealth _health;
    private PlayerDash _dash;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _shooting = GetComponent<PlayerShootingBehaviour>();
        _health = GetComponent<PlayerHealth>();
        
        _shooting.Initialize(gameStateEvent);
        
        _dash = GetComponent<PlayerDash>();
        if (_dash != null) _dash.enabled = playerStats.hasDash;
    }

    private void OnEnable()
    {
        gameStateEvent.OnRaised += HandleStateChanged;
        _health.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        gameStateEvent.OnRaised -= HandleStateChanged;
        _health.OnDeath -= HandlePlayerDeath;
    }

    private void HandleStateChanged(GameState state)
    {
        Time.timeScale = state == GameState.Paused ? 0f : 1f;
        
        _controller.SetGameState(state);
        _shooting.SetGameState(state);
        _dash?.SetGameState(state);
    }

    private void HandlePlayerDeath()
    {
        GameManager.Instance.ChangeState(GameState.GameOver);
    }
}
