using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class JoyStick_Movement : MonoBehaviour
{
   
    Vector3 movementInput;
    public Rigidbody playerRigidbody;

    void Start()
    {
        //playerRigidbody = GetComponent<Rigidbody>();
      //  playerRigidbody.freezeRotation = true;
    }

    public void move(Vector3 inputVector)
    {

        movementInput = new Vector3(inputVector.x, 0f, inputVector.y);

        //movementInput = Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward;
        movementInput.Normalize();

        float y = playerRigidbody.linearVelocity.y;

        if (movementInput != Vector3.zero)
        {
            if (transform.forward != movementInput)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movementInput), Time.deltaTime * 180);

                playerRigidbody.linearVelocity = Vector3.MoveTowards(playerRigidbody.linearVelocity, Vector3.zero, Time.deltaTime * 30);
            }
            else
            {
                playerRigidbody.linearVelocity = Vector3.MoveTowards(playerRigidbody.linearVelocity, movementInput * 10, Time.deltaTime * 30);
            }
        }
        else
        {
            playerRigidbody.linearVelocity = Vector3.MoveTowards(playerRigidbody.linearVelocity, Vector3.zero, Time.deltaTime * 30);
        }
        Vector3 velocity = playerRigidbody.linearVelocity;
        velocity.y = y;
        playerRigidbody.linearVelocity = velocity;
    }
}