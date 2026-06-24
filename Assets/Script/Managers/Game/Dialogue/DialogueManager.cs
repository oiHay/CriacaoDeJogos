using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")] 
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Typewriter")] 
    [SerializeField] private float letterDelay = 0.05f;

    private string[] _lines;
    private int _currentLine;
    private bool _isTyping;
    private Coroutine _typingCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        _lines = dialogue.GetLines();
        if (_lines == null || _lines.Length == 0) return;

        _currentLine = 0;
        dialoguePanel.SetActive(true);
        GameManager.Instance.ChangeState(GameState.Tutorial);
        ShowLine(_currentLine);
    }

    public void Advance()
    {
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            dialogueText.text = _lines[_currentLine];
            _isTyping = false;
            return;
        }

        _currentLine++;

        if (_currentLine < _lines.Length)
            ShowLine(_currentLine);
        else
            EndDialogue();
    }

    private void ShowLine(int index)
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypeLine(_lines[index]));
    }

    private IEnumerator TypeLine(string line)
    {
        _isTyping = true;
        dialogueText.text = string.Empty;

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(letterDelay);
        }

        _isTyping = false;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        _lines = null;
        GameManager.Instance.ChangeState(GameState.Playing);
    }
}
