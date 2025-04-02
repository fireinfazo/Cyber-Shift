using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicTracks;

    private AudioSource _audioSource;
    private int _currentTrackIndex;
    private bool _initialized;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;

        // Отложенная инициализация на случай, если SettingsManager ещё не создан
        StartCoroutine(InitializeWhenReady());
    }

    private System.Collections.IEnumerator InitializeWhenReady()
    {
        while (SettingsManager.Instance == null)
        {
            yield return null;
        }

        SettingsManager.Instance.OnSettingsChanged += UpdateMusicSettings;
        _initialized = true;
        UpdateMusicSettings();
    }

    private void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnSettingsChanged -= UpdateMusicSettings;
        }
    }

    private void Update()
    {
        if (!_initialized) return;

        if (ShouldPlayNextTrack())
        {
            PlayNextTrack();
        }
    }

    private bool ShouldPlayNextTrack()
    {
        return SettingsManager.Instance.IsMusicEnabled &&
               musicTracks != null &&
               musicTracks.Length > 0 &&
               !_audioSource.isPlaying;
    }

    private void PlayNextTrack()
    {
        if (musicTracks == null || musicTracks.Length == 0) return;

        _audioSource.clip = musicTracks[_currentTrackIndex];
        _audioSource.volume = SettingsManager.Instance.MusicVolume;
        _audioSource.Play();

        _currentTrackIndex = (_currentTrackIndex + 1) % musicTracks.Length;
    }

    private void UpdateMusicSettings()
    {
        if (!_initialized) return;

        _audioSource.volume = SettingsManager.Instance.MusicVolume;

        if (!SettingsManager.Instance.IsMusicEnabled)
        {
            _audioSource.Stop();
        }
        else if (!_audioSource.isPlaying && musicTracks != null && musicTracks.Length > 0)
        {
            PlayNextTrack();
        }
    }
}