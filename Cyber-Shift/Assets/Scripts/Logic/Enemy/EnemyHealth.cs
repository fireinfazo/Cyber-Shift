using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float armorPercentage = 0f;
    private int currentHealth;

    public AudioClip hitSound;
    private AudioSource audioSource;
    private bool isPlayingHitSound = false;

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

        //Debug.Log($"Враг получил {reducedDamage} урона. Осталось здоровья: {currentHealth}");

        if (audioSource != null && hitSound != null && !isPlayingHitSound)
        {
            isPlayingHitSound = true;
            audioSource.PlayOneShot(hitSound);
            StartCoroutine(ResetHitSoundFlag(hitSound.length));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ResetHitSoundFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayingHitSound = false;
    }

    void Die()
    {
        //Debug.Log("Враг уничтожен");
        Destroy(gameObject);
    }
}