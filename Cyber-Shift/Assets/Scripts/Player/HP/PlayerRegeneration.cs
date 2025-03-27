using UnityEngine;
using System.Collections;

public class PlayerRegeneration : MonoBehaviour
{
    [SerializeField] private int healAmount = 5;
    [SerializeField] private int maxRegenHealth = 40;
    [SerializeField] private float healInterval = 3f;

    private bool isHealing = false;

    private void Start()
    {
        StartCoroutine(RegenerateHealth());
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);

            int currentHealth = PlayerHealth.Instance.GetHealth();
            //int maxHealth = 100;

            if (currentHealth < maxRegenHealth)
            {
                int newHealth = Mathf.Min(currentHealth + healAmount, maxRegenHealth);
                PlayerHealth.Instance.Heal(newHealth - currentHealth);
            }
        }
    }
}
