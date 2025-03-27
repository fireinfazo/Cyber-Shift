using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            PlayerHealth.Instance.TakeDamage(bulletDamage);
            Destroy(other.gameObject);
        }
    }
}
