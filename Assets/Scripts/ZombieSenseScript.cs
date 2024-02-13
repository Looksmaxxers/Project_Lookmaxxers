using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSenseScript : MonoBehaviour
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
                zombie.OnSensePlayer(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (zombie)
            {
                zombie.OnLosePlayer(other.gameObject);
            }
        }
    }
}
