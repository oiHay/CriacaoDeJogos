using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private GameManager gameManager;

    private PlayerController _controller;
    private PlayerShootingBehaviour _shooting;
    private PlayerHealth _health;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _shooting = GetComponent<PlayerShootingBehaviour>();
        _health = GetComponent<PlayerHealth>();
        
        _shooting.Initialize(gameStateEvent);
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
    }

    private void HandlePlayerDeath()
    {
        gameManager.ChangeState(GameState.GameOver);
    }
}
