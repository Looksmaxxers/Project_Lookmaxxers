using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private int[] hitEnemies; // Array to keep track of enemies hit by the weapon
    private Collider weaponCollider;

    public string againstTag;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        hitEnemies = new int[0];
        weaponCollider = GetComponentInChildren<Collider>();
        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (
                other.gameObject.tag == againstTag && 
                ArrayUtility.IndexOf(hitEnemies, other.gameObject.GetInstanceID()) == -1
            )
        {
            hitEnemies.Append(other.gameObject.GetInstanceID());
            IEntityStats stats = other.gameObject.GetComponent<IEntityStats>();
            stats.TakeDamage(damage);
        }
    }

    public void OnAttackBegin()
    {
        hitEnemies = new int[0];
        weaponCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        weaponCollider.enabled = false;
    }

}
