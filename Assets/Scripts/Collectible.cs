using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<CharacterStats>().AddFlask();
            Destroy(gameObject);
        }
    }
}
