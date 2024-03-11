using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAIScript;

public class EnemyAIScript : MonoBehaviour
{
    public enum AIState
    {
        PATROL,
        SEEK,
        ATTACK,
        RETREAT,
    }


    public float maxLookAheadTime;
    public GameObject[] waypoints;
    public GameObject target;
    public Vector3 targetFuturePosition;

    private EnemyStats stats;
    private bool hasSearched = true;
    private bool isWaiting = false;
    private float patrolWaitTime = 5f;
    private IEntityStats targetStats;
    NavMeshAgent agent;
    Animator animator;

    AIState aiState;
    int currentWaypoint = -1;

    private void Awake()
    {
        if (waypoints.Length == 0)
        {
            GameObject waypoint = new GameObject();
            waypoint.transform.position = transform.position;

            waypoints = new GameObject[1];
            waypoints[0] = waypoint;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();

        aiState = AIState.PATROL;
        setNextWayPoint();

        if (target != null)
        {
            targetStats = target.GetComponent<IEntityStats>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("vely", agent.velocity.magnitude / agent.speed);

        switch (aiState)
        {
            case AIState.PATROL:
                animator.SetBool("aggressive", false);
                animator.SetBool("attack", false);
                if (!isWaiting && agent.remainingDistance < 1f && !agent.pathPending)
                {
                    isWaiting = true;
                    StartCoroutine(PatrolWait());
                }
                if (hasSearched)
                {
                    hasSearched = false;
                    StartCoroutine(SearchForTarget());
                }
                break;
            case AIState.SEEK:
                animator.SetBool("aggressive", true);
                animator.SetBool("attack", false);
                setDestinationToPredicted();

                if (stats.curStamina > stats.GetWeaponScript().staminaCost)
                {
                    if (!stats.isStaggered && Vector3.Distance(transform.position, target.transform.position) < 1.8f && !agent.pathPending)
                    {
                        aiState = AIState.ATTACK;
                        animator.SetBool("attack", true);
                        // aiState = AIState.PATROL;
                        // setNextWayPoint();
                    }
                } else
                {
                    stats.RerollRetreatThreshold();
                    aiState = AIState.RETREAT;
                    agent.updateRotation = false;
                }
                break;
            case AIState.RETREAT:
                animator.SetBool("attack", false);
                if (stats.curStamina >= stats.retreatThreshold)
                {
                    agent.updateRotation = true;
                    aiState = AIState.SEEK;
                }
                Vector3 directionAwayFromTarget = transform.position - target.transform.position;
                Vector3 retreatPosition = transform.position + directionAwayFromTarget.normalized * stats.retreatDistance;
                agent.speed = stats.retreatSpeed;
                agent.SetDestination(retreatPosition);
                transform.rotation = Quaternion.LookRotation(new Vector3(-directionAwayFromTarget.x, 0, -directionAwayFromTarget.z));
                break;
            case AIState.ATTACK:
                setDestinationToPredicted();
                break;
            default:
                break;
        }
    }

    void setDestinationToPredicted()
    {
        //float dist = Vector3.Distance(target.transform.position, transform.position);

        //NavMeshHit hit;
        //bool blocked = NavMesh.Raycast(transform.position, target.transform.position, out hit, NavMesh.AllAreas);

        //if (blocked)
        //{
        //    futureTarget = Vector3.Lerp(hit.position, target.transform.position, 0.29f);
        //}

        //targetFuturePosition = futureTarget;

        agent.SetDestination(target.transform.position);
    }

    void setNextWayPoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].transform.position);
            targetFuturePosition = waypoints[currentWaypoint].transform.position;
        }
        else
        {
            Debug.Log("No waypoints set as waypoints is of size 0");
        }
    }

    public void ChangeToAISeek()
    {
        aiState = AIState.SEEK;
    }

    IEnumerator PatrolWait()
    {
        yield return new WaitForSeconds(patrolWaitTime);
        isWaiting = false;
        if (aiState == AIState.PATROL)
        {
            setNextWayPoint();
        }
    }

    IEnumerator SearchForTarget()
    {
        Debug.Log("Searching for target");
        yield return new WaitForSeconds(0.5f);
        if (target != null)
        {
            NavMeshHit hit;

            bool blocked = NavMesh.Raycast(transform.position, target.transform.position, out hit, NavMesh.AllAreas);

            Debug.Log("Blocked: " + blocked);

            // draw the NavMesh raycast
            Debug.DrawRay(transform.position, target.transform.position - transform.position, blocked ? Color.red : Color.green, 1f);

            if (!blocked)
            {
                aiState = AIState.SEEK;
            }
        }
        hasSearched = true;
    }
}
