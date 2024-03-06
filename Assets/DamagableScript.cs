using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DamagableScript : MonoBehaviour
{

    private IEntityStats entityStats;

    void Start()
    {
        entityStats = gameObject.GetComponent<IEntityStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("DamagableScript: OnTriggerEnter");
        if (other.gameObject.tag == "Attack")
        {
            Debug.Log("DamagableScript: OnTriggerEnter");
            WeaponScript weapon = other.gameObject.GetComponentInParent<WeaponScript>();
            entityStats.TakeDamage(weapon.damage);
        }
    }
}
