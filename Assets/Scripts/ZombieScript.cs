using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class ZombieScript : MonoBehaviour
{

    private Animator anim;
    private Rigidbody rbody;
    
    public GameObject[] seenPlayers;
    public GameObject[] attackPlayers;

    private int groundContactCount = 0;

    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    public bool seesPlayer = false;
    public bool isAttacking = false;
    public float forwardMaxSpeed = 1f;
    public float turnMaxSpeed = 1f;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("[Zombie] Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("[Zombie] Rigid body could not be found");

        anim.applyRootMotion = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputForward = 0f;
        float inputTurn = 0f;
        float vely = 0f;

        if (seesPlayer && seenPlayers.Length > 0)
        {
            Vector3 direction = (seenPlayers[0].transform.position - this.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // we want to rotate only around y axis
            targetRotation.x = 0;
            targetRotation.z = 0;
            
            float newVelY = Vector3.Dot(rbody.velocity, transform.forward);

            inputForward = newVelY;

            //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
            //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
            //Therefore, an additional raycast approach is used to check for close ground
            //bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

            //We use rbody.MovePosition() as it's the most efficient and safest way to directly control position in Unity's Physics
            float velyDebt = isAttacking ? 0.5f : 1f;


            rbody.MovePosition(rbody.position + transform.forward * velyDebt * Time.deltaTime * forwardMaxSpeed);
            //Most characters use capsule colliders constrained to not rotate around X or Z axis
            //However, it's also good to freeze rotation around the Y axis too. This is because friction against walls/corners
            //can turn the character. This errant turn is disorienting to players. 
            //Luckily, we can break the frozen Y axis constraint with rbody.MoveRotation()
            //BTW, quaternions multiplied has the effect of adding the rotations together
            
                rbody.MoveRotation(Quaternion.Slerp(rbody.rotation, targetRotation, Time.deltaTime * turnMaxSpeed));

            // anim.SetFloat("velx", inputTurn); 
            vely = 1f;
            anim.SetBool("SensePlayer", true);
            //anim.SetBool("isFalling", !isGrounded);
        } 
        else
        {
            anim.SetBool("SensePlayer", false);
        }

        if (isAttacking && attackPlayers.Length > 0)
        {
            anim.SetBool("AttackPlayer", true);
        }
        else
        {
            anim.SetBool("AttackPlayer", false);
        }

        //Debug.Log(rbody.velocity.magnitude);

        anim.SetFloat("vely", Mathf.Lerp(anim.GetFloat("vely"), vely, 3f * Time.deltaTime));
    }

    public void OnSensePlayer(GameObject player)
    {
        seenPlayers = seenPlayers.Concat(new GameObject[] { player }).ToArray();
        seesPlayer = true;
    }

    public void OnLosePlayer(GameObject player)
    {
        seenPlayers = seenPlayers.Where(p => p != player).ToArray();
        if (seenPlayers.Length == 0)
        {
            seesPlayer = false;
        }
    }

    public void OnAttackPlayer(GameObject player)
    {
        attackPlayers = attackPlayers.Concat(new GameObject[] { player }).ToArray();
        isAttacking = true;
    }
    
    public void OnStopAttackPlayer(GameObject player)
    {
        attackPlayers = attackPlayers.Where(p => p != player).ToArray();
        if (attackPlayers.Length == 0)
        {
            isAttacking = false;
        }
    }
}
