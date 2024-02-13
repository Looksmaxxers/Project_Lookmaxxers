using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBeetleScript : MonoBehaviour
{
    public bool onGround = false;
    public float minRandomTime = 1f;
    public float maxRandomTime = 5f;
    public float minRandomTorque = 10f;
    public float maxRandomTorque = 50f;
    public float minRandomForce = 10f;
    public float maxRandomForce = 50f;
    public Vector3 minTorqueAngle = new Vector3(0, 0, 0);
    public Vector3 maxTorqueAngle = new Vector3(0, 0, 0);
    public Vector3 minForceAngle = new Vector3(0, 0, 0);
    public Vector3 maxForceAngle = new Vector3(0, 0, 0);

    public float nextTime = 0.0f;
    public float nextTorque = 0.0f;
    public float nextForce = 0.0f;
    public Vector3 nextTorqueAngle = new Vector3(0, 0, 0);
    public Vector3 nextForceAngle = new Vector3(0, 0, 0);

    void Start()
    {
        nextTime = Random.Range(minRandomTime, maxRandomTime);
        nextTorque = Random.Range(minRandomTorque, maxRandomTorque);
        nextForce = Random.Range(minRandomForce, maxRandomForce);
        nextTorqueAngle = new Vector3(Random.Range(minTorqueAngle.x, maxTorqueAngle.x), Random.Range(minTorqueAngle.y, maxTorqueAngle.y), Random.Range(minTorqueAngle.z, maxTorqueAngle.z));
        nextForceAngle = new Vector3(Random.Range(minForceAngle.x, maxForceAngle.x), Random.Range(minForceAngle.y, maxForceAngle.y), Random.Range(minForceAngle.z, maxForceAngle.z));
    }

    void FixedUpdate()
    {
        if (Time.time > nextTime)
        {
            if (onGround) {
                onGround = false;
                nextTorque = Random.Range(minRandomTorque, maxRandomTorque);
                nextForce = Random.Range(minRandomForce, maxRandomForce);
                nextTorqueAngle = new Vector3(Random.Range(minTorqueAngle.x, maxTorqueAngle.x), Random.Range(minTorqueAngle.y, maxTorqueAngle.y), Random.Range(minTorqueAngle.z, maxTorqueAngle.z));
                nextForceAngle = new Vector3(Random.Range(minForceAngle.x, maxForceAngle.x), Random.Range(minForceAngle.y, maxForceAngle.y), Random.Range(minForceAngle.z, maxForceAngle.z));

                GetComponent<Rigidbody>().AddForce(Vector3.up * nextForce);
                GetComponent<Rigidbody>().AddTorque(Vector3.forward * nextTorque);
            }
            nextTime = Time.time + Random.Range(minRandomTime, maxRandomTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            onGround = true;
        }
    }
}
