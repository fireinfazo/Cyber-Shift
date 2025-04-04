using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider dialogueVolumeSlider;

    private void OnEnable()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager not found!");
            return;
        }

        UpdateUI();

        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
        dialogueVolumeSlider.onValueChanged.AddListener(OnDialogueVolumeSliderChanged);
        SettingsManager.Instance.OnSettingsChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (musicToggle != null)
            musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeSliderChanged);

        if (dialogueVolumeSlider != null)
            dialogueVolumeSlider.onValueChanged.RemoveListener(OnDialogueVolumeSliderChanged);

        if (SettingsManager.Instance != null)
            SettingsManager.Instance.OnSettingsChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (musicToggle != null && volumeSlider != null && dialogueVolumeSlider != null)
        {
            musicToggle.isOn = SettingsManager.Instance.IsMusicEnabled;
            volumeSlider.value = SettingsManager.Instance.MusicVolume;
            dialogueVolumeSlider.value = SettingsManager.Instance.DialogueVolume;
        }
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        SettingsManager.Instance.SetMusicEnabled(isOn);
    }

    private void OnVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }

    private void OnDialogueVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.SetDialogueVolume(value);
    }
}