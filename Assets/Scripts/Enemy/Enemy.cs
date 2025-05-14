using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealthy
{
    public enum State
    {
        Attacking,
        Chasing,
        Searching
    }

    [SerializeField] private Transform target;
    [SerializeField] public State state = State.Searching;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float searchDistance = 20f;
    [SerializeField] private float pushForce = 4f;
    [SerializeField] private float pushUpForce = 1f;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float avoidanceRadius = 10f;
    [SerializeField] private float avoidanceForce = 4f;
    [SerializeField] private float hordeRadius = 10f;

    private bool busy = false;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath path;
    private Animator anim;

    private Player_Stats playerStats;
    public TurretPartDropper partDropper;
    
    private Enemy_Stats enemyStats;
    private bool isDead = false;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        path = new NavMeshPath();
        
        enemyStats = GetComponent<Enemy_Stats>();
        if (enemyStats == null)
        {
            Debug.LogError("Enemy_Stats component not found on the enemy object.");
        }
        else
        {
            enemyStats.OnDeath.AddListener(HandleDeath);
        }

        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        isDead = false;
        busy = false;
        state = State.Searching;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            playerStats = player.GetComponent<Player_Stats>();
        }
        else
        {
            Debug.LogError("Player object not found. Make sure the player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (busy || isDead)
        {
            navMeshAgent.isStopped = true;
            return;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }

        DistanceToTarget();

        switch (state)
        {
            case State.Attacking:
                anim.SetBool("Walking", false);
                anim.SetBool("Sprinting", false);
                anim.SetBool("Taunting", true);
                StartCoroutine(Attack());
                break;
            case State.Chasing:
                anim.SetBool("Walking", false);
                anim.SetBool("Sprinting", true);
                Chase();
                break;
            case State.Searching:
                anim.SetBool("Sprinting", false);
                anim.SetBool("Walking", true);
                Search();
                break;
        }
    }

    private IEnumerator Attack()
    {
        busy = true;

        yield return new WaitForSeconds(attackTime);

        float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer <= attackDistance)
        {
            Vector3 pushDirection = (target.position - transform.position).normalized;
            Vector3 pushForceVector = (pushDirection + Vector3.up * pushUpForce) * pushForce;
            playerStats.Damage(10, pushForceVector);
        }

        busy = false;
        state = State.Chasing;
        anim.SetBool("Taunting", false);
    }

    private void Chase()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            Vector3 avoidanceVector = CalculateAvoidanceVector();
            
            Vector3 finalDestination = targetPosition + avoidanceVector;
            navMeshAgent.SetDestination(finalDestination);
        }
    }

    private Vector3 CalculateAvoidanceVector()
    {
        if (IsPathToPlayerShort())
        {
            return Vector3.zero; // No avoidance when path is short
        }

        Vector3 avoidanceVector = Vector3.zero;
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, avoidanceRadius, enemyLayer);

        foreach (Collider enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject != gameObject)
            {
                Vector3 awayFromEnemy = transform.position - enemyCollider.transform.position;
                avoidanceVector += awayFromEnemy.normalized / awayFromEnemy.magnitude;
            }
        }

        return avoidanceVector * avoidanceForce;
    }

    private void Search()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 5f, 1);
            Vector3 finalPosition = hit.position;

            Vector3 avoidanceVector = CalculateAvoidanceVector();
            finalPosition += avoidanceVector;

            navMeshAgent.SetDestination(finalPosition);
        }
    }

    private void DistanceToTarget()
    {
        float difference = Vector3.Distance(transform.position, target.position);

        switch (state)
        {
            case State.Attacking:
                if (difference > attackDistance)
                {
                    state = State.Chasing;
                }
                break;
            case State.Chasing:
                if (difference <= attackDistance)
                {
                    state = State.Attacking;
                }
                else if (!PathDistance(searchDistance))
                {
                    state = State.Searching;
                }
                break;
            case State.Searching:
                if (PathDistance(searchDistance))
                {
                    state = State.Chasing;
                }
                break;
        }
    }

    private bool PathDistance(float distance)
    {
        if (navMeshAgent.CalculatePath(target.position, path))
        {
            float pathLength = 0f;

            if (path.corners.Length < 2)
                return false;

            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            return pathLength <= distance;
        }

        return false;
    }

    private bool IsPathToPlayerShort()
    {
        if (navMeshAgent.CalculatePath(target.position, path))
        {
            float pathLength = 0f;

            if (path.corners.Length < 2)
                return false;

            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                if (pathLength > hordeRadius)
                    return false;
            }

            return true;
        }

        return false;
    }
    
    private void HandleDeath()
    {
        isDead = true;
        // anim.SetBool("Dead", true); // TODO: Add death animation
        navMeshAgent.isStopped = true;
        
        if (partDropper != null)
        {
            partDropper.DropRandomPart(transform.position);
            playerStats.killCount++;
            Debug.Log($"Kills: {playerStats.killCount}");
        }
    }

    public void doDamage(float damage)
    {
        enemyStats.Damage((int)damage);
    }

    public void healHealth(float health)
    {
        // Implement healing logic if needed
        enemyStats.currentHealth += (int)health;
        enemyStats.currentHealth = Mathf.Clamp(enemyStats.currentHealth, enemyStats.minHealth, enemyStats.maxHealth);
    }

    public void setHealth(float health)
    {
        enemyStats.currentHealth = (int)health;
    }

    public IEnumerator Confuse(float duration) {
        float counter = 0;
        Debug.Log($"beginning confusion on {this} for {duration} seconds");

        while (counter <= duration)
        {
            counter += Time.deltaTime;

            target = GameObject.FindGameObjectWithTag("Enemy").transform;

            yield return null;
        }

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}