using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if (debugMode)
            Debug.Log(message);
    }

    #endregion
    
    public static LocalizationManager Instance { get; private set; }

    [Header("Languages")] 
    [SerializeField] private List<LanguageDataSO> availableLanguages;

    [SerializeField] private Languages defaultLanguage = Languages.Portuguese;

    public event Action OnLanguageChanged;
    public Languages CurrentLanguage { get; private set; }

    private LanguageDataSO _currentData;
    private const string LangPrefKey = "Language";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadSavedLanguage();
    }

    private void LoadSavedLanguage()
    {
        Languages saved = (Languages)PlayerPrefs.GetInt(LangPrefKey, (int)defaultLanguage);
        SetLanguage(saved);
    }

    public void SetLanguage(Languages language)
    {
        LanguageDataSO data = availableLanguages.Find(l => l.languages == language);

        if (data == null)
        {
            DebugMessage("Language data not found for: " + language);
            return;
        }

        CurrentLanguage = language;
        _currentData = data;
        _currentData.BuildLookup();
        
        PlayerPrefs.SetInt(LangPrefKey, (int)language);
        OnLanguageChanged?.Invoke();
        
        DebugMessage("Language changed to: " + language);
    }

    public string GetText(string key)
    {
        if (_currentData == null) return "[" + key + "]";
        return _currentData.GetString(key);
    }
}
