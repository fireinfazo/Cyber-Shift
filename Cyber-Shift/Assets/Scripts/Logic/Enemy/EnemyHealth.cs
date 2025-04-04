using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float armorPercentage = 0f;
    private int currentHealth;
    private bool isDead = false;

    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;
    private bool isPlayingHitSound = false;

    [Header("Death Settings")]
    [SerializeField] private float destroyDelay = 60f;
    [SerializeField] private Collider[] hitboxesToDisable;

    [Header("Animation")]
    [SerializeField] private string deathAnimationTrigger = "Die";
    [SerializeField] private string walkingAnimationParam = "Walking";
    [SerializeField] private string attackingAnimationParam = "Attacking";

    private EnemyAi enemyAi;
    private Animator animator;
    private NavMeshAgent navAgent;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        enemyAi = GetComponent<EnemyAi>();
        animator = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        // Автоматически находим все коллайдеры если не заданы вручную
        if (hitboxesToDisable == null || hitboxesToDisable.Length == 0)
        {
            hitboxesToDisable = GetComponentsInChildren<Collider>();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        if (isDead) return;

        int reducedDamage = Mathf.RoundToInt(damage * (1f - armorPercentage / 100f));
        currentHealth -= reducedDamage;

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
        isDead = true;

        if (enemyAi != null) enemyAi.enabled = false;
        if (navAgent != null) navAgent.enabled = false;

        foreach (var hitbox in hitboxesToDisable)
        {
            hitbox.enabled = false;
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (animator != null)
        {
            animator.SetBool(walkingAnimationParam, false);
            animator.SetBool(attackingAnimationParam, false);

            animator.ResetTrigger(deathAnimationTrigger); 
            animator.SetTrigger(deathAnimationTrigger);

            animator.enabled = false;
            animator.enabled = true;
        }

        StartCoroutine(ForceDeathAnimation());

        StartCoroutine(DestroyAfterDelay(destroyDelay));
    }

    IEnumerator ForceDeathAnimation()
    {
        if (animator != null)
        {
            yield return null;

            animator.Play("Death", 0, 0f);
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}