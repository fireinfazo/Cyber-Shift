using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    [SerializeField] private int maxHealth = 100;
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
        Debug.Log("Остання путь");
    }
}