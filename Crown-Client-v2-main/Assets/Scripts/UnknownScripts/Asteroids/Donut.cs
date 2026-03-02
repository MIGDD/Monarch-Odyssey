using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }


    //freeze donut after hitting ground
    void OnCollisionEnter (Collision collide)
    {
        Rigidbody donut = GetComponent<Rigidbody>();

        if(collide.gameObject.name == "Ground")
        {
            donut.isKinematic = false;
        }
    }
}
