using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SettingsManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SettingsManager");
                    _instance = obj.AddComponent<SettingsManager>();
                    DontDestroyOnLoad(obj);
                    _instance.LoadSettings();
                }
            }
            return _instance;
        }
    }

    public bool IsMusicEnabled { get; private set; } = true;
    public float MusicVolume { get; private set; } = 0.5f;

    public event System.Action OnSettingsChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    public void SetMusicEnabled(bool enabled)
    {
        IsMusicEnabled = enabled;
        SaveSettings();
        OnSettingsChanged?.Invoke();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);
        SaveSettings();
        OnSettingsChanged?.Invoke();
    }

    private void LoadSettings()
    {
        IsMusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicEnabled", IsMusicEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }
}