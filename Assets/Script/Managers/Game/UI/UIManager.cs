using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Debug

        [SerializeField] private bool debugMode;

        private void DebugMessage(string message)
        {
            if(debugMode)
                Debug.Log(message);
        }

    #endregion
    
    [Header("Reference")]
    [SerializeField] private GameStatesEventSO gameStateEvent; // Referência direta o GameStateEventSO

    [Header("Panels")] 
    [SerializeField] private GameObject settingsPanel; // Refêrencia ao panel que deve aparecer durante o estado de Pause do jogo
    [SerializeField] private GameObject choosingPowerUp;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    
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
        if (settingsPanel == null || choosingPowerUp == null || gameOverPanel == null || victoryPanel == null) return; // Proteção contra referências destruídas, evita o erro MissingReferenceException
        
        settingsPanel.SetActive(state == GameState.Paused);
        choosingPowerUp.SetActive(state == GameState.ChoosingPowerUp);
        gameOverPanel.SetActive(state == GameState.GameOver);
        victoryPanel.SetActive(state == GameState.Victory);
    }

    public void ResumeGame()   => GameManager.Instance.ResumeGame();
    public void ResetScene()   => GameManager.Instance.ResetScene();
    public void GoToMainMenu() => GameManager.Instance.GoToMainMenu();
}
