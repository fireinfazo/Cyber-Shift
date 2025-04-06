using UnityEngine;

public class LightningObstacle : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float activeDuration = 3f;
    [SerializeField] private float inactiveDuration = 2f;
    [SerializeField] private float damageInterval = 0.3f;

    [Header("References")]
    [SerializeField] private ParticleSystem lightningParticles;
    [SerializeField] private Collider hitCollider;
    [SerializeField] private AudioSource lightningAudioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip backgroundSound;

    private bool isActive = true;
    private float timer = 0f;
    private float damageTimer = 0f;
    private bool soundInterrupted = false;
    private bool playerInside = false;
    private IHealthSystem playerHealthSystem;

    private void Start()
    {
        SetActiveState(true);
        timer = activeDuration;

        if (lightningAudioSource != null && backgroundSound != null)
        {
            lightningAudioSource.clip = backgroundSound;
            lightningAudioSource.loop = true;
            lightningAudioSource.Play();
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = !isActive;
            SetActiveState(isActive);
            timer = isActive ? activeDuration : inactiveDuration;
        }

        if (isActive && soundInterrupted && !lightningAudioSource.isPlaying)
        {
            lightningAudioSource.Play();
            soundInterrupted = false;
        }

        if (isActive && playerInside)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                ApplyDamage();
                damageTimer = damageInterval;
            }
        }
    }

    private void SetActiveState(bool active)
    {
        if (lightningParticles != null)
        {
            if (active) lightningParticles.Play();
            else lightningParticles.Stop();
        }

        if (hitCollider != null)
        {
            hitCollider.enabled = active;
        }

        if (lightningAudioSource != null && !soundInterrupted)
        {
            if (active && !lightningAudioSource.isPlaying)
                lightningAudioSource.Play();
            else if (!active)
                lightningAudioSource.Pause();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            playerInside = true;
            playerHealthSystem = other.GetComponent<IHealthSystem>();
            damageTimer = damageInterval;

            ApplyDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealthSystem = null;
        }
    }

    private void ApplyDamage()
    {
        if (playerHealthSystem != null)
        {
            playerHealthSystem.TakeDamage(damageAmount);

            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);

                if (lightningAudioSource != null && lightningAudioSource.isPlaying)
                {
                    lightningAudioSource.Stop();
                    soundInterrupted = true;
                }
            }
        }
    }
}