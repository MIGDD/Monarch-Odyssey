using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movementv2 : MonoBehaviour
{
    [SerializeField] float movementSpeed = 100;
    [SerializeField] float turnSpeed = 60;

    Transform myT;




    private void Awake()
    {
        myT = transform;
    }
    void Update()
    {


        Thrust();
        Turn();

    }

    void Turn()
    {
        float yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float pitch = turnSpeed * Time.deltaTime * Input.GetAxis("Pitch");

        myT.Rotate(pitch, yaw, 0);
    }



    void Thrust()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
    }



}
