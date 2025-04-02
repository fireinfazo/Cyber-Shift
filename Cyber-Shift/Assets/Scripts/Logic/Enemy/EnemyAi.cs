using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint;

    public LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;

    public float health;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange, detectionAngle = 90f;
    public bool playerInSightRange, playerInAttackRange, playerVisible;

    [Header("Animation")]
    [SerializeField] private GameObject modelObject;
    private Animator animator;
    private static readonly int WalkingParam = Animator.StringToHash("Walking");
    private static readonly int AttackingParam = Animator.StringToHash("Attacking");

    private void Awake()
    {
        player = GameObject.Find("player").transform;
        agent = GetComponent<NavMeshAgent>();

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
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        int attempts = 0;
        bool validPointFound = false;

        while (attempts < 10 && !validPointFound)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            {
                Vector3 directionToWalkPoint = (walkPoint - transform.position).normalized;
                float distanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);

                if (!Physics.Raycast(transform.position, directionToWalkPoint, distanceToWalkPoint, whatIsObstacle))
                {
                    walkPointSet = true;
                    validPointFound = true;
                }
            }

            attempts++;
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

            Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * 32f, ForceMode.Impulse);
            rb.AddForce(firePoint.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
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
}