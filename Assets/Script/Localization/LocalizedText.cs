using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;

    private TMP_Text _text;
    private bool _start;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (LocalizationManager.Instance == null) return;

        LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        UpdateText();
        _start = true;
    }

    private void OnEnable()
    {
        if (!_start || LocalizationManager.Instance == null) return;
        
        LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        if (LocalizationManager.Instance == null) return;

        LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        _text.text = LocalizationManager.Instance.GetText(key);
    }
}
