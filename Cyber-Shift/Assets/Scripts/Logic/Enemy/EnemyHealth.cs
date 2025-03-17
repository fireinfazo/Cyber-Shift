using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float armorPercentage = 0f;
    private int currentHealth;

    public AudioClip hitSound;  
    public AudioClip deathSound;  
    private AudioSource audioSource; 

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();  
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        int reducedDamage = Mathf.RoundToInt(damage * (1f - armorPercentage / 100f));
        currentHealth -= reducedDamage;

        Debug.Log($"Враг получил {reducedDamage} урона. Осталось здоровья: {currentHealth}");


        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Враг уничтожен");

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        StartCoroutine(DestroyAfterSound());
    }

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);

        Destroy(gameObject);
    }
}
