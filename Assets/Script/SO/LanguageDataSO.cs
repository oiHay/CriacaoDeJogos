using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Localization/Language Data")]
public class LanguageDataSO : ScriptableObject
{
    [System.Serializable]
    public class StringEntry
    {
        public string key;
        [TextArea] public string value;
    }

    public Languages languages;
    public List<StringEntry> entries = new();

    private Dictionary<string, string> _lookup;

    public void BuildLookup()
    {
        _lookup = new Dictionary<string, string>(entries.Count);
        foreach (var entry in entries)
            _lookup[entry.key] = entry.value;
    }

    public string GetString(string key)
    {
        if (_lookup == null) BuildLookup();
        return _lookup.TryGetValue(key, out var value) ? value : $"[{key}]";
    }
}
