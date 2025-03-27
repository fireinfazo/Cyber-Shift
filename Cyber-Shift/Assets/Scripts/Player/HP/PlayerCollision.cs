using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 10;

    private IHealthSystem healthSystem;

    private void Start()
    {
        healthSystem = PlayerHealth.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            healthSystem.TakeDamage(bulletDamage);
            Destroy(other.gameObject);
        }
    }
}