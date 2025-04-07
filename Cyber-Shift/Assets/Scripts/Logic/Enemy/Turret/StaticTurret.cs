using UnityEngine;

public class StaticTurret : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionAngle = 90f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleLayers;

    [Header("Combat Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float shootingDelay = 1f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip detectionSound;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip ambientSound;

    private bool isPlayerDetected = false;
    private bool isActive = true;
    private float nextFireTime = 0f;
    private float detectionTime = 0f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (audioSource != null && ambientSound != null)
        {
            audioSource.loop = true;
            audioSource.clip = ambientSound;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (!isActive || player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        bool canSeePlayer = distanceToPlayer <= detectionRange &&
                          angleToPlayer <= detectionAngle / 2f &&
                          !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayers);

        if (canSeePlayer)
        {
            if (!isPlayerDetected)
            {
                isPlayerDetected = true;
                detectionTime = Time.time;
                PlaySound(detectionSound);
            }

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Time.time - detectionTime >= shootingDelay && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            isPlayerDetected = false;
            detectionTime = 0f;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = firePoint.forward * bulletSpeed;
        }

        Destroy(bullet, bulletLifetime);

        PlaySound(shootingSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void DisableTurret()
    {
        isActive = false;
        if (audioSource != null) audioSource.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Vector3 leftBound = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRange;
        Vector3 rightBound = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRange;
        Gizmos.DrawLine(transform.position, transform.position + leftBound);
        Gizmos.DrawLine(transform.position, transform.position + rightBound);

        DrawViewAngleGizmo();
    }

    private void DrawViewAngleGizmo()
    {
        int segments = 30;
        float angleStep = detectionAngle / segments;
        Vector3 prevPoint = transform.position + Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRange;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -detectionAngle / 2 + angleStep * i;
            Vector3 nextPoint = transform.position + Quaternion.Euler(0, angle, 0) * transform.forward * detectionRange;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}