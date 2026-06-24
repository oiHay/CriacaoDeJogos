using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool triggerOnStart = false;

    private void Start()
    {
        if (triggerOnStart)
            Trigger();
    }

    public void Trigger()
    {
        StartCoroutine(TriggerAfterDelay());
    }

    private IEnumerator TriggerAfterDelay()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        DialogueManager.Instance.StartDialogue(dialogue);
    }
}