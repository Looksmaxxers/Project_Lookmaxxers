using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mikeAi : MonoBehaviour
{

    // fields
    public Transform player;
    private NavMeshAgent agent;
    public float attackRange;
    public bool firstContactWithPlayer;
    public Animator anim;
    public float maxDetectionRange;
    public GameObject head;
    public float chanceOfHeavyAttack;






    public enum State
    {
        Idle,
        Seeking,
        Attacking
    }

    public State currentState;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        // STATE MACHINE
        switch (currentState)
        {
            case State.Idle:
                // Debug.Log("Idle State");
                idle();

                break;

            case State.Seeking:
                // Debug.Log("Seeking State");
                seeking();

                break;

            case State.Attacking:
                // Debug.Log("Attacking State");
                attacking();

                break;
        }
        setSpeed();


    }



    // STATE MACHINE BEHAVIOURS 
    void idle()
    {
        if (CanSeePlayer())
        {
            // Change state to Seeking if player is seen
            ChangeState(State.Seeking);
        }
    }

    void seeking()
    {

        if (firstContactWithPlayer)
        {
            anim.SetTrigger("War Cry");
        }
        else
        {
            approachPlayer();

        }

        // this has been moved to the "war cry" animation clip
        // agent.SetDestination(player.position);

        // Check if player is within attack range
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            // Change state to Attacking if player is within attack range
            ChangeState(State.Attacking);

        }


    }

    void attacking()
    {
        if (Random.value > chanceOfHeavyAttack)
        {
            anim.SetTrigger("LightAttack");

        }
        else
        {
            anim.SetTrigger("HeavyAttack");

        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            // Change state to Attacking if player is within attack range
            ChangeState(State.Seeking);

        }

    }








    // helpers
    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    private bool CanSeePlayer()
    {
        // Define the direction from the agent to the player
        Vector3 direction = player.position - head.transform.position;

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(head.transform.position, direction, out hit))
        {
            // If the ray hits an object tagged as "Player", return true
            if (hit.collider.CompareTag("Player"))
            {
                // Draw a line from the agent to the player for visualization
                Debug.DrawLine(head.transform.position, player.position, Color.green);
                return true;
            }
        }

        // Draw a line from the agent to the maximum detection range for visualization
        Debug.DrawRay(head.transform.position, direction.normalized * maxDetectionRange, Color.red);

        // If the ray doesn't hit the player, return false
        return false;
    }

    private void approachPlayer()
    {
        agent.SetDestination(player.position);
        firstContactWithPlayer = false;

    }

    private void setSpeed()
    {
        anim.SetFloat("Blend", agent.velocity.magnitude);
    }


}
