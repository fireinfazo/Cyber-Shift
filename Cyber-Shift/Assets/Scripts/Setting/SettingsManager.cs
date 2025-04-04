using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;
    private static GameObject _settingsManagerObject;

    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                CreateInstance();
            }
            return _instance;
        }
    }

    public bool IsMusicEnabled { get; private set; } = true;
    public float MusicVolume { get; private set; } = 0.5f;
    public event System.Action OnSettingsChanged;

    private static void CreateInstance()
    {
        if (_settingsManagerObject != null)
        {
            DestroyImmediate(_settingsManagerObject);
            _settingsManagerObject = null;
            _instance = null;
        }

        _settingsManagerObject = new GameObject("SettingsManager");
        _instance = _settingsManagerObject.AddComponent<SettingsManager>();
        DontDestroyOnLoad(_settingsManagerObject);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        _settingsManagerObject = gameObject;
        DontDestroyOnLoad(gameObject);
        LoadSettings();

        SceneManager.sceneLoaded += OnSceneReloaded;
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        CleanupDuplicates();
    }

    private void CleanupDuplicates()
    {
        var managers = FindObjectsOfType<SettingsManager>();
        foreach (var manager in managers)
        {
            if (manager != _instance && manager.gameObject != null)
            {
                DestroyImmediate(manager.gameObject);
            }
        }
    }

    public void SetMusicEnabled(bool enabled)
    {
        if (IsMusicEnabled != enabled)
        {
            IsMusicEnabled = enabled;
            SaveSettings();
            OnSettingsChanged?.Invoke();
        }
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        if (MusicVolume != volume)
        {
            MusicVolume = volume;
            SaveSettings();
            OnSettingsChanged?.Invoke();
        }
    }

    private void LoadSettings()
    {
        IsMusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        OnSettingsChanged?.Invoke();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicEnabled", IsMusicEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneReloaded;
            _instance = null;
            _settingsManagerObject = null;
            OnSettingsChanged = null;
        }
    }

    public static void PrepareForSceneReload()
    {
        if (_settingsManagerObject != null)
        {
            Destroy(_settingsManagerObject);
            _settingsManagerObject = null;
            _instance = null;
        }
    }
}