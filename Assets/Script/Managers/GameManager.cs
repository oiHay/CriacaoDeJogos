using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if(debugMode)
            Debug.Log(message);
    }

    #endregion
    
    [SerializeField] private GameStatesEventSO gameStatesEvent;  // Referencia direta ao GameStateEventSO, permite que o código saiba qual é o estado atual do jogo e que o mesmo possa ser alterado

    public  static GameManager Instance { get; private set; }
    
    private GameState _previousState;
   

    private void Awake() // Singleton, permite que o game object que possui esse código persista durante loads da cena
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update() 
    { 
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameStatesEvent.gameStateAtual == GameState.Paused)
            {
                ChangeState(_previousState);
            }
            else
            {
                ChangeState(GameState.Paused);
            }
        }
    }

    public void ResumeGame()
    {
        ChangeState(_previousState);
    }

    // Scene navigation — instance wrappers so buttons can reference them in the Inspector
    public void ResetScene() => CustomSceneManager.Reset();
    public void GoToMainMenu() => CustomSceneManager.MainMenu();

    public void ChangeState(GameState newState) // público para permitir que outros scripts alterem o estado
    {
        _previousState = gameStatesEvent.gameStateAtual;
        gameStatesEvent.Raise(newState); // Método que permite que o gameManager mude o valor do estado atual da cena
        
        DebugMessage("Estado Atual do jogo: " + gameStatesEvent.gameStateAtual.ToString()); // Debug para saber o estado atual do jogo
    }
}