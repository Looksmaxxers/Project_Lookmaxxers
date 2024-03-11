using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private CharacterStats stats;

    private CharacterController _controller;
    private StarterAssets.ThirdPersonController tpc;
    void Start()
    {
        animator = GetComponent<Animator>();
        tpc = GetComponent<StarterAssets.ThirdPersonController>();
        stats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tpc.Grounded && Input.GetKeyDown(KeyCode.E) && stats.CanAttack() && !stats.isRolling && Time.timeScale != 0)
        {
            // Trigger the roll animation by setting the corresponding parameter in the Animator
            animator.SetBool("Roll", true);
        }
    }

    public void ResetRolling()
    {
        // Set the boolean parameter to false
        animator.SetBool("Roll", false);
    }
}
