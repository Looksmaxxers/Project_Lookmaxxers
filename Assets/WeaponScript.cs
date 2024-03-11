using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private List<int> hitEnemies; // Array to keep track of enemies hit by the weapon
    private Collider weaponCollider;
    public GameObject wielder = null;
    public GameObject[] weapons;
    public int currSelectedWeapon = 0;
    public string againstTag;
    public int damage;
    public int staminaCost = 0;

    public GameObject magicProj;
    // Start is called before the first frame update
    void Start()
    {
        hitEnemies = new List<int>();
        weaponCollider = GetComponentInChildren<Collider>();
        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeWeapon(int x) 
    {
        switch (x)
        {
            case 0:
                damage = 12;
                staminaCost = 10;
                break;
            case 1:
                damage = 6;
                staminaCost = 5;
                break;
        }
                
        weapons[currSelectedWeapon].SetActive(false);
        currSelectedWeapon = x; 
        weapons[x].SetActive(true);
        weaponCollider = GetComponentInChildren<Collider>();
    }

    private GameObject FindEntityWithStats(GameObject obj)
    {
        IEntityStats stats = obj.GetComponent<IEntityStats>();
        if (stats != null)
        {
            return obj;
        }
        else
        {
            return FindEntityWithStats(obj.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == againstTag)
        {
            GameObject enemy = FindEntityWithStats(other.gameObject);
            if (!hitEnemies.Contains(enemy.GetInstanceID()))
            {
                hitEnemies.Add(enemy.GetInstanceID());
                IEntityStats stats = enemy.GetComponent<IEntityStats>();
                stats.TakeDamage(damage);
            }
        }
    }

    public void SetWielder(GameObject wielder)
    {
        this.wielder = wielder;
    }

    public int HitEnemy(int enemyID)
    {
        hitEnemies.Append(enemyID);
        return damage;
    }


    public void OnAttackBegin()
    {
        hitEnemies = new List<int>();
        if (currSelectedWeapon != 2)
        {
            weaponCollider.enabled = true;
        } else if (currSelectedWeapon == 2)
        {
            GameObject proj = Instantiate(magicProj, weapons[currSelectedWeapon].transform.position + new Vector3(0, .5f, 0), new Quaternion());
            Vector3 projDir = (Quaternion.Euler(0.0f, wielder.transform.eulerAngles.y, 0.0f) * Vector3.forward).normalized;
            Debug.Log(projDir);
            proj.GetComponent<ProjectileScript>().SetDirection(projDir);
        }
    }

    public void OnAttackEnd()
    {
        weaponCollider.enabled = false;
    }

    public float GetStaminaCost()
    {
        return staminaCost;
    }
}
