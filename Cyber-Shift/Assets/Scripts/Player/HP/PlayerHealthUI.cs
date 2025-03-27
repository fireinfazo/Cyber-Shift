using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour //������ ��������� ��� ����� ��!!!! 
{
    [SerializeField] private TextMeshProUGUI healthText;

    private void Update()
    {
        healthText.text = "HP: " + PlayerHealth.Instance.GetHealth();
    }
}
