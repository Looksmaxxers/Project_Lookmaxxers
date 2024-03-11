using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MinionAI;

public class MinionAI : MonoBehaviour
{
    public enum AIState
    {
        PATROL,
        SEEK,
    }


    public float maxLookAheadTime;
    public GameObject[] waypoints;
    public GameObject target;
    public Vector3 targetFuturePosition;


    private IEntityStats targetStats;
    NavMeshAgent agent;
    Animator animator;

    AIState aiState;
    int currentWaypoint = -1;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        aiState = AIState.SEEK;
        setNextWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("vely", agent.velocity.magnitude / agent.speed);

        switch (aiState)
        {
            case AIState.PATROL:
                if (agent.remainingDistance == 0 && !agent.pathPending)
                {
                    currentWaypoint++;
                    if (currentWaypoint == waypoints.Length)
                    {
                        if (target)
                        {
                            aiState = AIState.SEEK;
                            currentWaypoint = -1;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        setNextWayPoint();
                    }
                }
                break;
            case AIState.SEEK:
                setDestinationToPredicted();
                if (Vector3.Distance(transform.position, target.transform.position) < 1f && !agent.pathPending)
                {
                    aiState = AIState.PATROL;
                    currentWaypoint++;
                    setNextWayPoint();
                }
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
        currentWaypoint = currentWaypoint % waypoints.Length;
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
}
