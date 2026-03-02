using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour
{

    private CharacterController _charController;
    public float rotation_speed;
    public float movement_speed;


    private Transform meshPlayer;
    private managerJoystick _mngrJoystick;
    private float inputX;
    private float inputZ;
    private Vector3 v_movement;
    private float moveSpeed;

    public SpawnObjects spawnObjects;
    public LaunchProjectile launchProjectile;

    private int Player_score = 0;
    [SerializeField]
    Text Player_count;


    private void Start()
    {

        Player_count.text = "Player: " + 0;

         moveSpeed = 1f;
         _mngrJoystick = GameObject.Find("imgJoystickBg").GetComponent<managerJoystick>();
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        meshPlayer = tempPlayer.transform.GetChild(0);

        //might be the problem 

        _charController = tempPlayer.GetComponent<CharacterController>();
    }

    private void Update() //used WASD for now, buttons can be added to UI for movement Later
    {
       //force for the front or back of player
       if (Input.GetKey(KeyCode.W))
            GetComponent<Rigidbody>().AddForce(transform.forward * movement_speed * Time.deltaTime);
        
       if (Input.GetKey(KeyCode.S))
            GetComponent<Rigidbody>().AddForce(transform.forward * -movement_speed * Time.deltaTime);
    
        //player rotation
       if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * rotation_speed*Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(-Vector3.up * rotation_speed * Time.deltaTime);


        //joystick
        inputX = _mngrJoystick.inputHorizontal();
        inputZ = _mngrJoystick.inputVertical();


        //Raycasting for collection and score count



        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f))
        {


            if (hit.collider.tag == "Asteroid")
            {


                Destroy(hit.collider.transform.parent.gameObject);
                Player_score++;
                Player_count.text = "Player: " + Player_score.ToString();

                spawnObjects.spawnedObjects.RemoveAt(spawnObjects.spawnedObjects.Count - 1);


            }
            if (hit.collider.tag == "Donut" && hit.collider.GetComponent<Rigidbody>().velocity.y == 0f)
            {
                Destroy(hit.collider.transform.gameObject);
                launchProjectile.canfire = true;
            }


        }




    }
    private void FixedUpdate()
    {
        v_movement = new Vector3 (inputX * moveSpeed, 0, inputZ * moveSpeed);
        _charController.Move(v_movement);

        //mesh rotate
        Vector3 lookDir = new Vector3(v_movement.x, 0, v_movement.z);
        meshPlayer.rotation = Quaternion.LookRotation(lookDir);
    }



}