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
    [SerializeField] private int maxFlasks;
    [SerializeField] private Scrollbar HSlider;
    [SerializeField] private Scrollbar SSlider;
    [SerializeField] private TMP_Text FlaskNumDisplay;
    [SerializeField] private GameObject curBonfire;
    [SerializeField] private float staggerThreshold;
    [SerializeField] private float staminaRecoverScalar;

    private Animator anim;
    private CharacterController cController;
    private ThirdPersonController tController;
    private WeaponScript weaponScript;
    private StarterAssetsInputs sai;


    public bool isRolling = false;
    public bool isAttacking = false;
    public bool isStaggered = false;
    public bool isDead = false;
    public bool isSprinting = false;
    public float defaultStaggerThreshold = 8f;
    public GameObject weaponRoot;
    public float cooldownTimer = 0.0f;
    public float recoverScalar = 20.0f;


    public GameObject vfxSlashObj;
    public GameObject slashRoot;

    void Awake()
    {
        weaponScript = weaponRoot.GetComponent<WeaponScript>();
        weaponScript.SetWielder(gameObject);
        vfxSlashObj.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cController = GetComponent<CharacterController>();
        tController = GetComponent<ThirdPersonController>();
        weaponScript = weaponRoot.GetComponent<WeaponScript>();
        sai = GetComponent<StarterAssetsInputs>();
        staggerThreshold = defaultStaggerThreshold;
    }
    public bool spendStamina(float spentValue)
    {
        float netStamina = curStamina - spentValue;
        curStamina = netStamina >= 0 ? netStamina : curStamina;
        return netStamina >= 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        
        HSlider.size = (curHealth / maxHealth);
        SSlider.size = (curStamina / maxStamina);

        FlaskNumDisplay.text = flaskNum.ToString();
        staggerThreshold = Mathf.Max(defaultStaggerThreshold, staggerThreshold - 1f * Time.deltaTime);

        isRolling = anim.GetBool("Roll");
        isAttacking = anim.GetBool("Attack");
        isSprinting = sai.sprint;
        //isStaggered = anim.GetBool("Stagger");

        if (isDead)
        {
            isDead = false;
            StartCoroutine(Die());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage((int)maxHealth);
        }

        //if (!isRolling && !isStaggered && !isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    anim.SetBool("Attack", true);
        //}
        // To change the currently selected weapon

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponScript.changeWeapon(0);
            anim.SetInteger("WeaponID", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponScript.changeWeapon(1);
            anim.SetInteger("WeaponID", 2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponScript.changeWeapon(2);
            anim.SetInteger("WeaponID", 1);
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.timeScale != 0)
        {
            Heal();
        }

        recoverStamina();

        if (isSprinting && !isRolling)
        {
            if (curStamina >= 0)
            {
                curStamina -= (3.0f * Time.deltaTime);
            }
        }


        // if (!isRolling && !isStaggered && !isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     anim.SetBool("Attack", true);
        // }

    }
    public void recoverStamina()
    {
        if (isRolling || isAttacking || isSprinting)
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
        staggerThreshold += (damage / 12f) * (defaultStaggerThreshold / staggerThreshold);
        if (curHealth <= 0)
        {
            isDead = true;
        }
        else if (damage >= staggerThreshold)
        {
            anim.SetTrigger("Stagger");
        }
    }

    public int HitEnemy(int enemyID)
    {
        return weaponScript.HitEnemy(enemyID);
    }

    public void OnAttackBegin()
    {
        Debug.Log("Attack Begin");
        weaponScript.OnAttackBegin();

        if (weaponScript.currSelectedWeapon != 2)
        {
            vfxSlashActive();
        }
    }

    public void OnAttackEnd()
    {
        Debug.Log("Attack End");
        weaponScript.OnAttackEnd();
        vfxSlashObj.SetActive(false);
    }

    public bool CanAttack()
    {
        return !isRolling && !isStaggered && !isAttacking;
    }

    public void vfxSlashActive()
    {
        //vfxSlashObj.transform.position = slashRoot.transform.position;
        vfxSlashObj.SetActive(true);
    }

    public IEnumerator Die()
    {
        tController.enabled = false;
        cController.enabled = false;
        yield return new WaitForFixedUpdate();
        anim.enabled = false;
        yield return new WaitForSeconds(5);

        Respawn();
    }

    public void AddFlask()
    {
        maxFlasks += 1;
        flaskNum += 1;
    }

    public void Heal()
    {
        if (flaskNum > 0)
        {
            curHealth += 30;
            if (curHealth > 100)
            {
                curHealth = 100;
            }
            flaskNum -= 1;
        }
    }

    public void Rest()
    {
        flaskNum = maxFlasks;
        curHealth = maxHealth;
    }

    private void StaggerBehaviour()
    {
        anim.SetTrigger("Stagger");
    }

    public void StartStagger()
    {
        isStaggered = true;
    }
    public void EndStagger()
    {
        isStaggered = false;
    }

    public void Respawn() {
        curHealth = maxHealth;
        flaskNum = maxFlasks;
        transform.position = curBonfire.transform.position + new Vector3(0, 0, 4);
        tController.enabled = true;
        cController.enabled = true;
        anim.enabled = true;
    }
}
