using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [System.Serializable]
    public class LocalizedLines
    {
        public Languages language;
        [TextArea] public string[] lines;
    }

    public LocalizedLines[] localizations;

    public string[] GetLines()
    {
        Languages current = LocalizationManager.Instance.CurrentLanguage;

        foreach (var localization in localizations)
            if (localization.language == current)
                return localization.lines;

        return localizations.Length > 0 ? localizations[0].lines : null;
    }
}
