using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeSlowdownDuration = 3f;
    [SerializeField] private float minTimeScale = 0.01f;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource audioSource;

    private int currentHealth;
    private bool isDying = false;
    private Coroutine slowdownCoroutine;

    private static PlayerHealth instance;
    public static PlayerHealth Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerHealth>();
            return instance;
        }
        private set => instance = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentHealth = maxHealth;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void IHealthSystem.TakeDamage(int damage)
    {
        if (isDying) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void IHealthSystem.Heal(int amount)
    {
        if (isDying) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    int IHealthSystem.GetHealth() => currentHealth;
    int IHealthSystem.GetMaxHealth() => maxHealth;

    private void Die()
    {
        if (isDying) return;
        isDying = true;

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (animator != null)
            animator.SetTrigger("Die");

        ShowGameOver();

        var movement = GetComponent<Movement>();
        if (movement != null)
            movement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (slowdownCoroutine != null)
            StopCoroutine(slowdownCoroutine);
        slowdownCoroutine = StartCoroutine(SlowdownTimeRoutine());
    }

    private IEnumerator SlowdownTimeRoutine()
    {
        float elapsed = 0f;
        float startTimeScale = Time.timeScale;

        while (elapsed < timeSlowdownDuration)
        {
            elapsed += Time.unscaledDeltaTime; 
            float progress = Mathf.Clamp01(elapsed / timeSlowdownDuration);
            Time.timeScale = Mathf.Lerp(startTimeScale, minTimeScale, progress);
            yield return null;
        }

        Time.timeScale = minTimeScale;
        //ShowGameOver();
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (slowdownCoroutine != null)
            StopCoroutine(slowdownCoroutine);

        SettingsManager.PrepareForSceneReload();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (Time.timeScale < 1f)
            Time.timeScale = 1f;
    }
}