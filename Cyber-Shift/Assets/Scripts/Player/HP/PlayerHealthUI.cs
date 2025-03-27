using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour //Скрипт верменный для теста хп!!!! 
{
    [SerializeField] private TextMeshProUGUI healthText;

    private void Update()
    {
        healthText.text = "HP: " + PlayerHealth.Instance.GetHealth();
    }
}
