using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private CharacterStats character;

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterStats>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Trigger the roll animation by setting the corresponding parameter in the Animator
            animator.SetTrigger("Attack");

        }
    }

    public void Attack()
    {
        Debug.Log("im gonna fucking buss");
        // Set the boolean parameter to false
        animator.SetTrigger("Attack");
        character.spendStamina(10);
    }
}

