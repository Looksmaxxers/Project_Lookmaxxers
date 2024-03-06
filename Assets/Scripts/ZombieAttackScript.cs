using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackScript : MonoBehaviour
{
    ZombieScript zombie;

    private void Awake()
    {
        zombie = GetComponentInParent<ZombieScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (zombie)
            {
                //Debug.Log("ZombieAttackScript: OnTriggerEnter");
                zombie.OnAttackPlayer(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (zombie)
            {
                zombie.OnStopAttackPlayer(other.gameObject);
            }
        }
    }
}
