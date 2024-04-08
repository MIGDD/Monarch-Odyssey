using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//from Tom Weiland's networking tutorial: https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5

public class JerryController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public Animator characterAnimator;
    public GameObject gpsTarget;
    public InputAction jerryControls;

    Vector2 moveDirection = Vector2.zero;



    private void Start()
    {
        //this script emulates movement if the game is being tested from Unity Editor
        //turn off otherwise

        if (!Application.isEditor)
        {
            this.enabled = false;
            return;
        }
        else
        {
            //gpsTarget.SetActive(false);
        }
    }


    private void OnEnable()
    {
        jerryControls.Enable();
    }

    private void OnDisable()
    {
        jerryControls.Disable();
    }
    private void Update()
    {

        if (GameManager.instance.returnGameState() != 0)
        {
            return;
        }

        float characterMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float characterRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        if (characterMovement > 0)
        {
            characterAnimator.SetBool("isMoving", true);
        }
        else
        {
            characterAnimator.SetBool("isMoving", false);
        }

        transform.Translate(0, 0, characterMovement);
        transform.Rotate(0, characterRotation, 0);
        gpsTarget.transform.position = transform.position;

    }

    //stuff commented out from networking tutorial.
    //private void FixedUpdate()
    //{
    //at fixed intervals, the client sends all the movement keypresses that players have taken to the server as an array of inputs.
    //SendInputToServer();
    //}
    /*  //commented out movement stuff because not needed for now
        private void SendInputToServer()
        {
            bool[] _inputs = new bool[]
            {
                Input.GetKey(KeyCode.UpArrow),
                Input.GetKey(KeyCode.DownArrow),
                Input.GetKey(KeyCode.RightArrow), 
                Input.GetKey(KeyCode.LeftArrow)
            };

            ClientSend.PlayerMovement(_inputs);

        }
    */
}
