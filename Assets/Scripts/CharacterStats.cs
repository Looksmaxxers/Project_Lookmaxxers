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
    [SerializeField] private int flaskNum;
    [SerializeField] private int maxFlasks;
    [SerializeField] private Scrollbar HSlider;
    [SerializeField] private TMP_Text FlaskNumDisplay;

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
    }

    // Update is called once per frame
    void Update()
    {
        HSlider.size = (curHealth / maxHealth);
        FlaskNumDisplay.text = flaskNum.ToString();

        isRolling = anim.GetBool("Roll");
        isAttacking = anim.GetBool("Attack");
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

        if (Input.GetKeyDown(KeyCode.R)) {
            Heal();
        }


        // if (!isRolling && !isStaggered && !isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     anim.SetBool("Attack", true);
        // }

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

    public void OnAttackBegin()
    {
        Debug.Log("Attack Begin");
        weaponScript.OnAttackBegin();
        vfxSlashActive();
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
        vfxSlashObj.transform.position = slashRoot.transform.position;
        vfxSlashObj.SetActive(true);
    }

    public IEnumerator Die()
    {
        tController.enabled = false;
        cController.enabled = false;
        yield return new WaitForFixedUpdate();
        anim.enabled = false;
    }

    public void AddFlask()
    {
        maxFlasks += 1;
        flaskNum += 1;
    }

    public void Heal()
    {
        if (flaskNum > 0) {
            curHealth += 30;
            if (curHealth > 100) {
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
}
