using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageSettingsUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;

    private void OnEnable()
    {
        PopulateDropdown();
        languageDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDisable()
    {
        languageDropdown.onValueChanged.RemoveListener(OnDropdownChanged);
    }

    private void PopulateDropdown()
    {
        languageDropdown.ClearOptions();

        var options = new List<string>();
        foreach (Languages lang in System.Enum.GetValues(typeof(Languages)))
            options.Add(lang.ToString());
        
        languageDropdown.AddOptions(options);
        
        languageDropdown.SetValueWithoutNotify((int)LocalizationManager.Instance.CurrentLanguage);
    }

    private void OnDropdownChanged(int index)
    {
        LocalizationManager.Instance.SetLanguage((Languages)index);
    }
}
