using UnityEngine;

public class MessagePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform player;
    [SerializeField] private AudioClip message;
    private float interactionDistance = 5f;
    private bool _initialized;

    private void Awake()
    {
        StartCoroutine(InitializeWhenReady());
    }

    private System.Collections.IEnumerator InitializeWhenReady()
    {
        while (SettingsManager.Instance == null)
        {
            yield return null;
        }

        SettingsManager.Instance.OnSettingsChanged += UpdateDialogueVolume;
        _initialized = true;
        UpdateDialogueVolume();
    }

    private void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnSettingsChanged -= UpdateDialogueVolume;
        }
    }

    void Update()
    {
        if (!_initialized) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(player.transform.position, player.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    PlayMessage();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void PlayMessage()
    {
        if (_audioSource != null && message != null)
        {
            _audioSource.volume = SettingsManager.Instance.DialogueVolume;
            _audioSource.PlayOneShot(message);
        }
    }

    private void UpdateDialogueVolume()
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.volume = SettingsManager.Instance.DialogueVolume;
        }
    }
}