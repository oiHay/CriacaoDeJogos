using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameStatesEventSO gameStateEvent;
    
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private float delay = 0f;

    private bool _hasTriggered;
    
    private void OnEnable() => gameStateEvent.OnRaised += HandleStateChanged;

    private void OnDisable() => gameStateEvent.OnRaised -= HandleStateChanged;

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Tutorial && !_hasTriggered)
        {
            _hasTriggered = true;
            StartCoroutine(TriggerAfterDelay());
        }
    }

    private IEnumerator TriggerAfterDelay()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        DialogueManager.Instance.StartDialogue(dialogue);
    }
}