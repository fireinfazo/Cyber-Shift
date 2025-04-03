using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject gameOverPanel;
    private int currentHealth;

    private static PlayerHealth instance;
    public static PlayerHealth Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHealth>();
            }
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
        {
            gameOverPanel.SetActive(false);
        }
    }

    void IHealthSystem.TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void IHealthSystem.Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    int IHealthSystem.GetHealth()
    {
        return currentHealth;
    }

    int IHealthSystem.GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}