using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameStateEvent")]
public class GameStatesEventSO : ScriptableObject
{
    public event Action<GameState> OnRaised; // Action que dita o estado que o jogo se encontra
    public GameState gameStateAtual; // Puxa os valores presentes dentro do enum de GameState para determinar em que estado o jogo se encontra

    private void OnEnable()
    {
        OnRaised = null; // Toda vez que é recriado ele limpa a lista de scripts que ouviam esse, serve para evitar o erro MissingReferenceException
    }

    public void Raise(GameState state)
    {
        gameStateAtual = state; // Variável que guarda na memória qual o estado atual do jogo
        OnRaised?.Invoke(state); // Notifica todos os scripts inscritos que o estado do jogo mudou
    }
}