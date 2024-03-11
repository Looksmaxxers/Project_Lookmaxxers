using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private List<int> hitEnemies; // Array to keep track of enemies hit by the weapon
    private Collider weaponCollider;
    private GameObject wielder = null;
    public GameObject[] weapons;
    private int currSelectedWeapon = 0;

    public string againstTag;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        hitEnemies = new List<int>();
        weaponCollider = GetComponentInChildren<Collider>();
        weaponCollider.enabled = false;
        changeWeapon(0);
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
                damage = 10;
                break;
            case 1:
                damage = 5;
                break;
        }
                
        weapons[currSelectedWeapon].SetActive(false);
        weapons[x].SetActive(true);
        currSelectedWeapon = x;
        weaponCollider = GetComponentInChildren<Collider>();
    }

    private GameObject FindEnemyWithStats(GameObject obj)
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
        Debug.Log(other.gameObject.tag);
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
        weaponCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        weaponCollider.enabled = false;
    }
}
