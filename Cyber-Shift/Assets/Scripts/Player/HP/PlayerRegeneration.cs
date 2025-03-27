using UnityEngine;
using System.Collections;

public class PlayerRegeneration : MonoBehaviour
{
    [SerializeField] private int healAmount = 5;
    [SerializeField] private int maxRegenHealth = 40;
    [SerializeField] private float healInterval = 3f;

    private IHealthSystem healthSystem;

    private void Start()
    {
        healthSystem = PlayerHealth.Instance;
        StartCoroutine(RegenerateHealth());
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);

            int currentHealth = healthSystem.GetHealth();

            if (currentHealth < maxRegenHealth)
            {
                int newHealth = Mathf.Min(currentHealth + healAmount, maxRegenHealth);
                healthSystem.Heal(newHealth - currentHealth);
            }
        }
    }
}