using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyStats : MonoBehaviour, IEntityStats
{
    [SerializeField]
    private float staminaRegenRate = 2f;
    [SerializeField]
    private float staminaRegenDelay = 0.7f;
    [SerializeField]
    private float staminaRegenDelayTimer = 0;

    private Animator anim;
    private Collider rootCollider;
    private EnemyAIScript controller;
    private Rigidbody rbody;
    private WeaponScript weaponScript;
    private NavMeshAgent navAgent;
    private float staggerThreshold;

    [HideInInspector]
    public bool isStaggered = false;
    [HideInInspector]
    public float retreatThreshold;
    [HideInInspector]
    public float agentSpeed;
    public GameObject weaponRoot;
    public float curHealth = 10;
    public float maxHealth = 10;
    public float curStamina = 10;
    public float maxStamina = 10;
    public float retreatSpeed = 1;
    public float retreatDistance = 2;
    public float defaultStaggerThreshold = 8f;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<EnemyAIScript>();
        rootCollider = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        weaponScript = weaponRoot.GetComponent<WeaponScript>();
        navAgent = GetComponent<NavMeshAgent>();
        agentSpeed = navAgent.speed;
        staggerThreshold = defaultStaggerThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        staggerThreshold = Mathf.Max(defaultStaggerThreshold, staggerThreshold - 1f * Time.deltaTime);

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
        staggerThreshold += (damage / 8f) * (defaultStaggerThreshold / staggerThreshold);
        if (curHealth <= 0)
        {
            isDead = true;
        }
        else if (damage >= staggerThreshold)
        {
            anim.SetTrigger("stagger");
        }
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

    public WeaponScript GetWeaponScript()
    {
        return weaponScript;
    }
    
    public void RerollRetreatThreshold()
    {
        retreatThreshold = Random.Range(0, maxStamina / 1.5f);
    }

    public void StartStagger()
    {
        isStaggered = true;
    }
    public void EndStagger()
    {
        isStaggered = false;
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
