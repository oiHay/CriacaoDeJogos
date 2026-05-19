using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameStatesEventSO gameStateEvent; // Referência direta o GameStateEventSO
    
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel; // Refêrencia ao panel que deve aparecer durante o estado de Pause do jogo
    [SerializeField] private GameObject gameOverPanel;

    private void OnEnable()
    {
        gameStateEvent.OnRaised += HandleStateChanged; // Inscreve esse script como ouvinte do GameStateEventSO enquanto o objeto estiver ativo
    }

    private void OnDisable()
    {
        gameStateEvent.OnRaised -= HandleStateChanged; // Remove esse script da lista de ouvintes quando o objeto for desativado ou destruído
    }

    private void HandleStateChanged(GameState state) // Método que serve como ouvinte do GameStateEventSO, toda vez que o GameState muda de valor, esse código verifica para qual mudou e faz o que for preciso referente a mudança
    {
        if (pausePanel == null || gameOverPanel == null) return; // Proteção contra referências destruídas, evita o erro MissingReferenceException
        
        pausePanel.SetActive(state == GameState.Paused); // Se o estado do jogo for "Paused", o pausePanel deve estar ativo
        gameOverPanel.SetActive(state == GameState.GameOver);
    }
}
