using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{

    public GameObject projectile;
    public float launchVelocity = 700f;
    public bool canfire;


    //[SerializeField] float laserOffTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        canfire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canfire == true)
        {
            Invoke("Throw", 0.0f);
        }

        

    }

    //chuck the donut, only one can be tossed
    public void Throw()
    {
        GameObject ball = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        ball.GetComponent<Rigidbody>().AddForce(transform.forward * -launchVelocity);

        canfire = false;

    }


}
