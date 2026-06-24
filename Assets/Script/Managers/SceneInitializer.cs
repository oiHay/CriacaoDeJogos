using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameState initialState = GameState.Playing;

    [SerializeField] private DialogueSO testDialogue;
    
    private void Start()
    {
        GameManager.Instance.ChangeState(initialState);
        
        DialogueManager.Instance.StartDialogue(testDialogue);
    }
}
