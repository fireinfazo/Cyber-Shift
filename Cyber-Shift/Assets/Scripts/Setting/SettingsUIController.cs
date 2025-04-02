using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Ожидаем инициализации SettingsManager
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager not initialized!");
            return;
        }

        musicToggle.isOn = SettingsManager.Instance.IsMusicEnabled;
        volumeSlider.value = SettingsManager.Instance.MusicVolume;

        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        SettingsManager.Instance.SetMusicEnabled(isOn);
    }

    private void OnVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }

    private void OnDestroy()
    {
        musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
        volumeSlider.onValueChanged.RemoveListener(OnVolumeSliderChanged);
    }
}