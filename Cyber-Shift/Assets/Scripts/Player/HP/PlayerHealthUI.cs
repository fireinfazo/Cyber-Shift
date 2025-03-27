using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    private IHealthSystem healthSystem;

    private void Start()
    {
        healthSystem = PlayerHealth.Instance;
    }

    private void Update()
    {
        healthText.text = $"HP: {healthSystem.GetHealth()}/{healthSystem.GetMaxHealth()}";
    }
}