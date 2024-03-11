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
    [SerializeField] private Scrollbar HSlider;
    [SerializeField] private TMP_Text FlaskNumDisplay;
    [SerializeField] private GameObject curBonfire;

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
            isDead = false;
            StartCoroutine(Die());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage((int)maxHealth);
        }
        
        if (!isRolling && !isStaggered && !isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("Attack", true);
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
        yield return new WaitForSeconds(5);

        Respawn();
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

    public void Respawn() {
        curHealth = maxHealth;
        flaskNum = maxFlasks;
        transform.position = curBonfire.transform.position + new Vector3(0, 0, 4);
        tController.enabled = true;
        cController.enabled = true;
        anim.enabled = true;
    }
}
