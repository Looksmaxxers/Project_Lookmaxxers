using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private CannonBallController controls;
    public GameObject projectile;
    public float launchVelocity = 700f;

    private void Awake()
    {
        controls = new CannonBallController();

        controls.Weapon.Fire.started += ctx => StartShot();
    }

    private void StartShot()
    {
        GameObject ball = Instantiate(projectile, transform.position, transform.rotation);

        ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, launchVelocity));
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
