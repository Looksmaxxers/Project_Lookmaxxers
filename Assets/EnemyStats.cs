using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyStats : MonoBehaviour, IEntityStats
{
    [SerializeField]
    private float staminaRegenRate = 1;
    [SerializeField]
    private float staminaRegenDelay = 1;
    [SerializeField]
    private float staminaRegenDelayTimer = 0;

    private Animator anim;
    private Collider rootCollider;
    private EnemyAIScript controller;
    private Rigidbody rbody;
    private WeaponScript weaponScript;
    private NavMeshAgent navAgent;

    public GameObject weaponRoot;
    public float curHealth = 10;
    public float maxHealth = 10;
    public float curStamina = 10;
    public float maxStamina = 10;

    public bool isDead = false;
    public int staggerThreshold = 5;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<EnemyAIScript>();
        rootCollider = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        weaponScript = weaponRoot.GetComponent<WeaponScript>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StartCoroutine(Die());
        }

        if (curStamina < maxStamina)
        {
            if (staminaRegenDelayTimer >= staminaRegenDelay)
            {
                curStamina += staminaRegenRate * Time.deltaTime;
            }
            else
            {
                staminaRegenDelayTimer += Time.deltaTime;
            }
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

    public float getStamina()
    {
        return curStamina;
    }

    public void OnAttackBegin()
    {
        curStamina -= weaponScript.GetStaminaCost();
        staminaRegenDelayTimer = 0;
        weaponScript.OnAttackBegin();
    }

    public void OnAttackEnd()
    {
        weaponScript.OnAttackEnd();
    }

    public IEnumerator Die()
    {
        controller.enabled = false;
        rootCollider.enabled = false;
        rbody.isKinematic = true;
        navAgent.enabled = false;
        yield return new WaitForFixedUpdate();
        anim.enabled = false;
    }
}
