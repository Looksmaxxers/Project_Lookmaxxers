using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyStats : MonoBehaviour, IEntityStats
{
    private Animator anim;
    private Collider rootCollider;
    private ZombieScript controller;
    private Rigidbody rbody;

    public float curHealth = 10;
    public float maxHealth = 10;
    public bool isDead = false;
    public int staggerThreshold = 5;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<ZombieScript>();
        rootCollider = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        //StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(int damage)
    {

        curHealth -= damage;
        if (curHealth <= 0)
        {
            isDead = true;
        }
        else if (damage >= staggerThreshold)
        {
            anim.SetTrigger("stagger");
        }
    }

    public IEnumerator Die()
    {
        controller.enabled = false;
        rootCollider.enabled = false;
        rbody.isKinematic = true;
        yield return new WaitForFixedUpdate();
        anim.enabled = false;
    }
}
