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

    public string againstTag;
    public int damage;

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

    private GameObject FindEnemyWithStats(GameObject obj)
    {
        IEntityStats stats = obj.GetComponent<IEntityStats>();
        if (stats != null)
        {
            return obj;
        }
        else
        {
            return FindEnemyWithStats(obj.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == againstTag)
        {
            GameObject enemy = FindEnemyWithStats(other.gameObject);
            if (!hitEnemies.Contains(enemy.GetInstanceID()))
            {
                hitEnemies.Add(enemy.GetInstanceID());
                Debug.Log(enemy.GetInstanceID());
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
        StartCoroutine(EnableHurtBox());
    }

    public void OnAttackEnd()
    {
        weaponCollider.enabled = false;
    }

    private IEnumerator EnableHurtBox()
    {
        yield return new WaitForSeconds(0.4f);
        weaponCollider.enabled = true;
        Debug.Log("WeaponScript: EnableHurtBox");
    }
}
