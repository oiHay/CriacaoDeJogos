using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;

    private PlayerController _controller;
    private PlayerShootingBehaviour _shooting;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _shooting = GetComponent<PlayerShootingBehaviour>();
        
        _shooting.Initialize(gameStateEvent);
    }

    private void OnEnable() => gameStateEvent.OnRaised += HandleStateChanged;
    private void OnDisable() => gameStateEvent.OnRaised -= HandleStateChanged;

    private void HandleStateChanged(GameState state)
    {
        Time.timeScale = state == GameState.Paused ? 0f : 1f;
        
        _controller.SetGameState(state);
        _shooting.SetGameState(state);
    }
}
