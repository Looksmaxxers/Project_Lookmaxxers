using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ladder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void moveLadder()
    {
        transform.position = new Vector3(transform.position.x, 1.22f, transform.position.z);
    }
}
