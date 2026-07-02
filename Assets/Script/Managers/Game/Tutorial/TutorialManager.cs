using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerSceneAnimation sceneAnimation;
    [SerializeField] private PlayerShootingBehaviour playerShooting;
    [SerializeField] private TutorialEnemyController tutorialEnemy;
    [SerializeField] private PlayerHealth playerHealth;
    
    [Header("Shooting Patterns")] 
    [SerializeField] private ShootingPatternSO emptyPattern;
    [SerializeField] private ShootingPatternSO enemyShootPattern;
    [SerializeField] private ShootingPatternSO playerShootPattern;

    [Header("Phase 1 - Move")] 
    [SerializeField] private DialogueSO phase1DialogueEntry;
    [SerializeField] private DialogueSO phase1DialogueObjective;
    [SerializeField] private TutorialObjective phase1Objective;

    [Header("Phase 2 - Dodge")]
    [SerializeField] private DialogueSO phase2DialogueEntry;
    [SerializeField] private DialogueSO phase2DialogueObjective;
    [SerializeField] private TutorialObjective phase2Objective;
    
    [Header("Phase 3 - Kill")]
    [SerializeField] private DialogueSO phase3DialogueEntry;
    [SerializeField] private TutorialObjective phase3Objective;
    
    [Header("Phase 4")]
    [SerializeField] private DialogueSO phase4DialogueEntry;
    [SerializeField] private DialogueSO phase4DialogueExit;

    private bool _dialogueEnded;
    private bool _objectiveCompleted;
    private bool _powerUpChosen;

    private void OnEnable() => DialogueManager.OnDialogueEnded += OnDialogueEnded;
    private void OnDisable() => DialogueManager.OnDialogueEnded -= OnDialogueEnded;

    private void OnDialogueEnded() => _dialogueEnded = true;
    private void OnPowerUpPicked() => _powerUpChosen = true;

    private void Start() => StartCoroutine(Phase1Routine());

    private IEnumerator Phase1Routine()
    {
        // Diálogo de entrada
        _dialogueEnded = false;
        
        DialogueManager.Instance.StartDialogue(phase1DialogueEntry);
        yield return new WaitUntil(() => _dialogueEnded);
        
        // Entrada do player na cena
        bool entryDone = false;
        sceneAnimation.PlayEnterAnimation(() => entryDone = true);
        yield return new WaitUntil(() => entryDone);

        // Diálogo de objetivo
        _dialogueEnded = false;
        
        DialogueManager.Instance.StartDialogue(phase1DialogueObjective);
        yield return new WaitUntil(() => _dialogueEnded);
        
        // Define o Shooting Pattern
        playerShooting.SetPattern(emptyPattern);
        tutorialEnemy.SetPattern(emptyPattern);
        
        // Jogo começa
        GameManager.Instance.ChangeState(GameState.Playing);
        
        // Objetivo
        _objectiveCompleted = false;
        phase1Objective.OnCompleted += OnObjectiveCompleted;
        phase1Objective.StartObjective();
        yield return new WaitUntil(() => _objectiveCompleted);
        phase1Objective.OnCompleted -= OnObjectiveCompleted;
        
        // Volta estado de tutorial
        GameManager.Instance.ChangeState(GameState.Tutorial);
        
        // Player volta a posição central
        bool returnDone = false;
        sceneAnimation.PlayerReturnAnimation(() => returnDone = true);
        yield return new WaitUntil(() => returnDone);
        
        // Passa para próxima fase do tutorial
        StartCoroutine(Phase2Routine());
    }

    private IEnumerator Phase2Routine()
    {
        // Diálogo de entrada
        _dialogueEnded = false;
        
        DialogueManager.Instance.StartDialogue(phase2DialogueEntry);
        yield return new WaitUntil(() => _dialogueEnded);
        
        // Inimigo entra na cena
        bool enemyEntryDone = false;
        tutorialEnemy.PlayEntryAnimation(() => enemyEntryDone = true);
        yield return new WaitUntil(() => enemyEntryDone);
        
        // Diálogo de objetivo
        _dialogueEnded = false;
        
        DialogueManager.Instance.StartDialogue(phase2DialogueObjective);
        yield return new WaitUntil(() => _dialogueEnded);
        
        // Define o Shooting Pattern
        playerShooting.SetPattern(emptyPattern);
        tutorialEnemy.SetPattern(enemyShootPattern);
        playerHealth.SetIndestructible(true);
        
        // Jogo começa
        GameManager.Instance.ChangeState(GameState.Playing);
        
        // Objetivo
        _objectiveCompleted = false;
        phase2Objective.OnCompleted += OnObjectiveCompleted;
        phase2Objective.StartObjective();
        yield return new WaitUntil(() => _objectiveCompleted);
        phase2Objective.OnCompleted -= OnObjectiveCompleted;
         
        // Volta estado de tutorial
        GameManager.Instance.ChangeState(GameState.Tutorial);
        playerHealth.SetIndestructible(false);
        playerHealth.HealToFull();
        
        // Player volta a posição central
        bool returnDone = false;
        sceneAnimation.PlayerReturnAnimation(() => returnDone = true);
        yield return new WaitUntil(() => returnDone);
        
        // Passa para próxima fase do tutorial
        StartCoroutine(Phase3Routine());
    }

    private IEnumerator Phase3Routine()
    {
        // Diálogo de entrada
        _dialogueEnded = false;
        
        DialogueManager.Instance.StartDialogue(phase3DialogueEntry);
        yield return new WaitUntil(() => _dialogueEnded);
        
        // Define o Shooting Pattern
        playerShooting.SetPattern(playerShootPattern);
        tutorialEnemy.SetPattern(emptyPattern);
        
        // Jogo começa
        GameManager.Instance.ChangeState(GameState.Playing);
        
        // Objetivo
        _objectiveCompleted = false;
        phase3Objective.OnCompleted += OnObjectiveCompleted;
        phase3Objective.StartObjective();
        yield return new WaitUntil(() => _objectiveCompleted);
        phase3Objective.OnCompleted -= OnObjectiveCompleted;
         
        // Volta estado de tutorial
        GameManager.Instance.ChangeState(GameState.Tutorial);
        
        // Player volta a posição central
        bool returnDone = false;
        sceneAnimation.PlayerReturnAnimation(() => returnDone = true);
        yield return new WaitUntil(() => returnDone);
        
        // Passa para próxima fase do tutorial
        StartCoroutine(Phase4Routine());
    }

    private IEnumerator Phase4Routine()
    {
        // diálogo de entrada
        _dialogueEnded = false;
        DialogueManager.Instance.StartDialogue(phase4DialogueEntry);
        yield return new WaitUntil(() => _dialogueEnded);
        
        yield return new WaitForSeconds(0.5f);

        // escolha de powerup
        _powerUpChosen = false;
        PowerUpManager.OnPowerUpPicked += OnPowerUpPicked;
        GameManager.Instance.ChangeState(GameState.ChoosingPowerUp);
        yield return new WaitUntil(() => _powerUpChosen);
        PowerUpManager.OnPowerUpPicked -= OnPowerUpPicked;
        
        GameManager.Instance.ChangeState(GameState.Tutorial);
        yield return new WaitForSeconds(0.75f);

        // animação de saída
        bool exitDone = false;
        sceneAnimation.PlayExitAnimation(() => exitDone = true);
        yield return new WaitUntil(() => exitDone);

        // diálogo de saída
        _dialogueEnded = false;
        DialogueManager.Instance.StartDialogue(phase4DialogueExit);
        yield return new WaitUntil(() => _dialogueEnded);

        yield return new WaitForSeconds(0.5f);
        
        // próximo nível
        CustomSceneManager.LoadNextScene();
    }

    private void OnObjectiveCompleted() => _objectiveCompleted = true;
}
