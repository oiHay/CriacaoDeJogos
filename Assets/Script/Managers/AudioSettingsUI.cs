using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambienceSlider;

    private void OnEnable()
    {
        LoadCurrentValues();

        masterSlider.onValueChanged.AddListener(OnMasterChanged);
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        ambienceSlider.onValueChanged.AddListener(OnAmbienceChanged);
    }

    private void OnDisable()
    {
        masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
        musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
        ambienceSlider.onValueChanged.RemoveListener(OnAmbienceChanged);
    }

    private void LoadCurrentValues()
    {
        if (AudioManager.Instance == null) return;

        masterSlider.SetValueWithoutNotify(AudioManager.Instance.GetMasterVolume());
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfxVolume());
        ambienceSlider.SetValueWithoutNotify(AudioManager.Instance.GetAmbienceVolume());
    }

    private void OnMasterChanged(float value) => AudioManager.Instance?.SetMasterVolume(value);
    private void OnMusicChanged(float value)  => AudioManager.Instance?.SetMusicVolume(value);
    private void OnSFXChanged(float value)    => AudioManager.Instance?.SetSfxVolume(value);
    private void OnAmbienceChanged(float value) => AudioManager.Instance?.SetAmbienceVolume(value);
}
