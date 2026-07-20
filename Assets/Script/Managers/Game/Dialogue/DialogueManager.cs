using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public static event Action OnDialogueEnded;

    [Header("UI")] 
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Typewriter")] 
    [SerializeField] private float letterDelay = 0.05f;

    [Header("Audio")] 
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioClip voiceBlip;
    [SerializeField, Range(0.5f, 1.5f)] private float minPitch = 0.85f;
    [SerializeField, Range(0.5f, 1.5f)] private float maxPitch = 1.2f;
    [SerializeField] private int lettersPerBlip = 2;

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
            if (_typingCoroutine != null)
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

        int letterCount = 0;

        foreach (char letter in line)
        {
            dialogueText.text += letter;

            if (char.IsLetter(letter) && letterCount++ % lettersPerBlip == 0)
                PlayBlip(letter);
            
            yield return new WaitForSecondsRealtime(letterDelay);
        }

        _isTyping = false;
    }

    private void PlayBlip(char letter)
    {
        if (voiceSource == null || !voiceSource.isActiveAndEnabled || voiceBlip == null) return;

        float t = Mathf.Clamp01((char.ToLowerInvariant(letter) - 'a') / 25f);
        voiceSource.pitch = Mathf.Lerp(minPitch, maxPitch, t) + Random.Range(-0.03f, 0.03f);
        voiceSource.PlayOneShot(voiceBlip);
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        _lines = null;
        OnDialogueEnded?.Invoke();
    }
}
