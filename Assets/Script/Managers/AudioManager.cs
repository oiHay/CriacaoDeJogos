using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Debug

    [SerializeField] private bool debugMode;

    private void DebugMessage(string message)
    {
        if (debugMode)
            Debug.Log(message);
    }

    #endregion

    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    
    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup ambienceGroup;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;

    private const string MasterParam = "MasterVolume";
    private const string MusicParam  = "MusicVolume";
    private const string SfxParam    = "SFXVolume";
    private const string AmbienceParam    = "AmbienceVolume";

    private const string MasterKey = "Audio_Master";
    private const string MusicKey  = "Audio_Music";
    private const string SfxKey    = "Audio_SFX";
    private const string AmbienceKey    = "Audio_Ambience";

    private readonly List<AudioSource> _ambienceSources = new();
    private const float AmbienceTrimDb = -10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;

        LoadSavedVolumes();
    }

    private void LoadSavedVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MasterKey, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MusicKey, 1f));
        SetSfxVolume(PlayerPrefs.GetFloat(SfxKey, 1f));
        SetAmbienceVolume(PlayerPrefs.GetFloat(AmbienceKey, 1f));
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(MasterParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(MasterKey, value);
        DebugMessage("Master Volume: " + value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MusicParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(MusicKey, value);
        DebugMessage("Music Volume: " + value);
    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat(SfxParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(SfxKey, value);
        DebugMessage("SFX Volume: " + value);
    }

    public void SetAmbienceVolume(float value)
    {
        audioMixer.SetFloat(AmbienceParam, LinearToDecibel(value) + AmbienceTrimDb);
        PlayerPrefs.SetFloat(AmbienceKey, value);
        DebugMessage("Ambience Volume: " + value);
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MasterKey, 1f);
    public float GetMusicVolume()  => PlayerPrefs.GetFloat(MusicKey,  1f);
    public float GetSfxVolume()    => PlayerPrefs.GetFloat(SfxKey,    1f);
    public float GetAmbienceVolume()    => PlayerPrefs.GetFloat(AmbienceKey,    1f);

    // Converts a 0–1 linear value to decibels (-80 dB to 0 dB)
    private float LinearToDecibel(float linear)
    {
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayAmbience(List<AudioClip> clips)
    {
        StopAmbience();
        if (clips == null) return;

        foreach (var clip in clips)
        {
            if (clip == null) continue;

            var sourceObject = new GameObject("Ambience_" + clip.name);
            sourceObject.transform.SetParent(transform);

            var source = sourceObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = true;
            source.outputAudioMixerGroup = ambienceGroup;
            source.Play();
            
            _ambienceSources.Add(source);
        }
    }

    public void StopAmbience()
    {
        foreach (var source in _ambienceSources)
            if (source != null) Destroy(source.gameObject);

        _ambienceSources.Clear();
    }
}
