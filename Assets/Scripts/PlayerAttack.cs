using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private CharacterStats characterStats;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && characterStats.CanAttack())
        {
            Attack();
        }
    }

    public void Attack()
    {
        // Set the boolean parameter to false
        animator.SetTrigger("Attack");
    }
}

