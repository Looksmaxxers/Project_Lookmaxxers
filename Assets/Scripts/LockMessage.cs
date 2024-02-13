using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockMessage : MonoBehaviour
{
    [SerializeField] TMP_Text doorText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        if(c.tag == "Player") {
            doorText.text = "Locked from this side.";
        }
    }

    void OnTriggerExit(Collider c) {
        if(c.tag == "Player") {
            doorText.text = "";
        }
    }
}
