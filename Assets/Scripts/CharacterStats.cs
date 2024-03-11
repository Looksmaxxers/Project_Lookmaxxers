using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class CharacterStats : MonoBehaviour, IEntityStats
{
    [SerializeField] private float curHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float curStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private int flaskNum;
    [SerializeField] private Scrollbar HSlider;
    [SerializeField] private Scrollbar SSlider;
    [SerializeField] private TMP_Text FlaskNumDisplay;
    [SerializeField] private float staminaRecoverScalar;

    private Animator anim;
    private CharacterController cController;
    private ThirdPersonController tController;
    private WeaponScript weaponScript;
        
    public bool isRolling = false;
    public bool isAttacking = false;
    public bool isStaggered = false;
    public bool isDead = false;
    public int staggerThreshold = 20;
    public GameObject weaponRoot;
    public float cooldownTimer = 0.0f;
    public float recoverScalar = 20.0f;

    void Awake()
    {
        weaponScript = weaponRoot.GetComponent<WeaponScript>();
        weaponScript.SetWielder(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        flaskNum = 5;
        anim = GetComponent<Animator>();
        cController = GetComponent<CharacterController>();
        tController = GetComponent<ThirdPersonController>();
        weaponScript = weaponRoot.GetComponent<WeaponScript>();
    }

    public void setCharacterHealth(float health)
    {
        curHealth = health;
    }

    public bool spendStamina(float spentValue)
    {
        float netStamina = curStamina - spentValue;
        curStamina = netStamina >= 0 ? netStamina : 0;
        return netStamina >= 0 ? true : false;
    }

    private bool isBehavioring()
    {
        return isRolling && isStaggered && isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        HSlider.size = (curHealth / maxHealth);
        SSlider.size = (curStamina / maxStamina);
        FlaskNumDisplay.text = flaskNum.ToString();

        isRolling = anim.GetBool("Roll");
        isAttacking = anim.GetBool("Attack");
        Debug.Log("Is there sprint: "+ anim.GetBool("Sprint"));
        //isStaggered = anim.GetBool("Stagger");

        if (isDead)
        {
            StartCoroutine(Die());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage((int)maxHealth);
        }
        
        if (!isRolling && !isStaggered && !isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (spendStamina(10))
            {
                anim.SetBool("Attack", true);
            }
        }

        recoverStamina();

    }

    public void recoverStamina()
    {
        if (isRolling || isAttacking)
        {
            cooldownTimer = 2;
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0)
        {
            if (curStamina < maxStamina)
            {
                curStamina += (recoverScalar * Time.deltaTime);
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
        else if (curHealth <= staggerThreshold)
        {
            anim.SetBool("Stagger", true);
        }
    }

    public int HitEnemy(int enemyID)
    {
        return weaponScript.HitEnemy(enemyID);
    }

    public IEnumerator Die()
    {
        tController.enabled = false;
        cController.enabled = false;
        yield return new WaitForFixedUpdate();
        anim.enabled = false;
    }

    public void OnAttackBegin()
    {
        Debug.Log("Attack Begin");
        weaponScript.OnAttackBegin();
    }

    public void OnAttackEnd()
    {
        Debug.Log("Attack End");
        weaponScript.OnAttackEnd();
    }
}
