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

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;

    private const string MasterParam = "MasterVolume";
    private const string MusicParam  = "MusicVolume";
    private const string SFXParam    = "SFXVolume";

    private const string MasterKey = "Audio_Master";
    private const string MusicKey  = "Audio_Music";
    private const string SFXKey    = "Audio_SFX";

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
        SetSFXVolume(PlayerPrefs.GetFloat(SFXKey, 1f));
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

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(SFXParam, LinearToDecibel(value));
        PlayerPrefs.SetFloat(SFXKey, value);
        DebugMessage("SFX Volume: " + value);
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MasterKey, 1f);
    public float GetMusicVolume()  => PlayerPrefs.GetFloat(MusicKey,  1f);
    public float GetSFXVolume()    => PlayerPrefs.GetFloat(SFXKey,    1f);

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
}
