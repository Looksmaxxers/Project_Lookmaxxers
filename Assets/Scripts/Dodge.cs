using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private CharacterStats character;
    private GameObject c;

    void Start()
    {
        animator = GetComponent<Animator>();
        c = GetComponent<GameObject>();
        character = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Trigger the roll animation by setting the corresponding parameter in the Animator
            animator.SetBool("Roll", true);
            character.spendStamina(10);
        }

    }

    public void ResetRolling()
    {
        // Set the boolean parameter to false
        animator.SetBool("Roll", false);
    }
}
