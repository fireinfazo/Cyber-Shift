using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public static PlayerHealth Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Остання путь");
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
