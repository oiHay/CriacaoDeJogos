using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerSceneAnimation sceneAnimation;

    [Header("Phase 1")] 
    [SerializeField] private DialogueSO phase1DialogueEntry;
    [SerializeField] private DialogueSO phase1DialogueObjective;
    [SerializeField] private TutorialObjective phase1Objective;

    [Header("Phase 2")]
    [SerializeField] private DialogueSO phase2DialogueEntry;
    [SerializeField] private DialogueSO phase2DialogueObjective;
    [SerializeField] private TutorialEnemyController phase2Enemy;
    [SerializeField] private TutorialObjective phase2Objective;

    private bool _dialogueEnded;
    private bool _objectiveCompleted;

    private void OnEnable() => DialogueManager.OnDialogueEnded += OnDialogueEnded;
    private void OnDisable() => DialogueManager.OnDialogueEnded -= OnDialogueEnded;

    private void OnDialogueEnded() => _dialogueEnded = true;

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

        TutorialEnemyController enemy = FindAnyObjectByType<TutorialEnemyController>();
        enemy.ShowEnemy();
        
        // Inimigo entra na cena
        bool enemyEntryDone = false;
        phase2Enemy.PlayEntryAnimation(() => enemyEntryDone = true);
        yield return new WaitUntil(() => enemyEntryDone);
        
        // Diálogo de objetivo
        _dialogueEnded = false;
        
        DialogueManager.Instance.StartDialogue(phase2DialogueObjective);
        yield return new WaitUntil(() => _dialogueEnded);
        
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
        
        // Player volta a posição central
        bool returnDone = false;
        sceneAnimation.PlayerReturnAnimation(() => returnDone = true);
        yield return new WaitUntil(() => returnDone);
        
        // Passa para próxima fase do tutorial
        //StartCoroutine(Phase3Routine());
    }

    private void OnObjectiveCompleted() => _objectiveCompleted = true;
}
