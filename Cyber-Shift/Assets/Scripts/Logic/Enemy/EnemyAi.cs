using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public enum WeaponType { Pistol, Automatic, Shotgun }
    public WeaponType weaponType = WeaponType.Pistol;

    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint;

    public LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange, detectionAngle = 90f;
    public bool playerInSightRange, playerInAttackRange, playerVisible;

    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    private float stuckCheckInterval = 1f;
    private float stuckThreshold = 0.1f;

    [Header("Weapon Settings")]
    [SerializeField] private float pistolFireRate = 1f;
    [SerializeField] private float automaticFireRate = 0.5f;
    [SerializeField] private float shotgunFireRate = 1.5f;
    [SerializeField] private int shotgunPelletCount = 5;
    [SerializeField] private float shotgunSpreadAngle = 15f;

    [Header("Animation")]
    [SerializeField] private GameObject modelObject;
    private Animator animator;
    private static readonly int WalkingParam = Animator.StringToHash("Walking");
    private static readonly int AttackingParam = Animator.StringToHash("Attacking");

    [Header("Sound Effects")]
    [SerializeField] private AudioClip pistolShotSound;
    [SerializeField] private AudioClip automaticShotSound;
    [SerializeField] private AudioClip shotgunShotSound;
    [SerializeField] private float shotVolume = 0.7f;
    private AudioSource audioSource;



    private void Awake()
    {
        player = GameObject.Find("player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
            audioSource.volume = shotVolume;
        }

        if (modelObject != null)
        {
            animator = modelObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Model object reference is not set in EnemyAi!");
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }

        switch (weaponType)
        {
            case WeaponType.Pistol:
                timeBetweenAttacks = pistolFireRate;
                break;
            case WeaponType.Automatic:
                timeBetweenAttacks = automaticFireRate;
                break;
            case WeaponType.Shotgun:
                timeBetweenAttacks = shotgunFireRate;
                break;
        }
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        playerVisible = CanSeePlayer();

        UpdateAnimations();

        if (playerInAttackRange)
        {
            AttackPlayer();
        }
        else if (playerInSightRange && playerVisible)
        {
            ChasePlayer();
        }
        else
        {
            Patroling();
        }

        if (Vector3.Distance(transform.position, lastPosition) < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckCheckInterval)
            {
                walkPointSet = false;
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = transform.position;
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool(WalkingParam, isMoving);
    }

    private bool CanSeePlayer()
    {
        if (!playerInSightRange) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (angleToPlayer < detectionAngle / 2f &&
            !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, whatIsObstacle))
        {
            return true;
        }

        return false;
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
            return;
        }

        agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < agent.stoppingDistance + 0.5f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        int attempts = 0;
        bool validPointFound = false;

        while (attempts < 20 && !validPointFound)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (NavMesh.SamplePosition(walkPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                walkPoint = hit.position;
                Vector3 directionToWalkPoint = (walkPoint - transform.position).normalized;

                if (!Physics.Raycast(transform.position, directionToWalkPoint,
                    Vector3.Distance(transform.position, walkPoint), whatIsObstacle))
                {
                    walkPointSet = true;
                    validPointFound = true;
                }
            }

            attempts++;
        }

        if (!validPointFound)
        {
            walkPoint = transform.position + Random.insideUnitSphere * walkPointRange;
            walkPoint.y = transform.position.y;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        Vector3 lookDirection = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookDirection);

        if (!alreadyAttacked && animator != null)
        {
            animator.SetTrigger(AttackingParam);

            switch (weaponType)
            {
                case WeaponType.Pistol:
                    FirePistol();
                    break;
                case WeaponType.Automatic:
                    FireAutomatic();
                    break;
                case WeaponType.Shotgun:
                    FireShotgun();
                    break;
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void FirePistol()
    {
        PlayShotSound(pistolShotSound);
        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * 32f, ForceMode.Impulse);
        rb.AddForce(firePoint.up * 8f, ForceMode.Impulse);
    }

    private void FireAutomatic()
    {
        PlayShotSound(automaticShotSound);
        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * 32f, ForceMode.Impulse);
        rb.AddForce(firePoint.up * 8f, ForceMode.Impulse);
    }

    private void FireShotgun()
    {
        PlayShotSound(shotgunShotSound);
        for (int i = 0; i < shotgunPelletCount; i++)
        {
            Vector3 spreadDirection = firePoint.forward;
            spreadDirection = Quaternion.AngleAxis(Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle), firePoint.up) * spreadDirection;
            spreadDirection = Quaternion.AngleAxis(Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle), firePoint.right) * spreadDirection;

            Rigidbody rb = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(spreadDirection)).GetComponent<Rigidbody>();
            rb.AddForce(spreadDirection * 32f, ForceMode.Impulse);
            rb.AddForce(firePoint.up * 8f, ForceMode.Impulse);
        }
    }

    private void PlayShotSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.cyan;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * sightRange;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * sightRange;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
        Gizmos.DrawLine(transform.position + leftBoundary, transform.position + rightBoundary);

        if (firePoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 0.5f);
        }
    }

    //хтось вдарте мене якщо я ще раз захочу писати ші мобам
}