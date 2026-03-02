/*using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using static JoyStick_Movement;


public class JoystickReader : MonoBehaviour
{
    public Vector2 touchDirection = Vector2.zero;
    JoyStick_Movement joystick_movement;


    private void Start()
    {
        //Subscribe to the action in JoyStick.cs
        joystick.onJoyStickMoved += GetJoyStickDirection;
    }

    void GetJoyStickDirection(Vector2 touchPosition)
    {
        //Touch direction updating every time joystick is moved.
        Vector3 touchDirection = touchPosition;
      //  touchDirection = touchPosition;
        //Call player move function here to move player.
        
        joystick_movement.move();
    }
}*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static JoyStick_Movement;
using System;


public class JoystickReader : MonoBehaviour
{
    public Vector2 touchDirection = Vector2.zero;
    JoyStick_Movement joystick_movement;
    public Rigidbody playerRigidbody; 

    private void Start()
    {
        // Create an instance of the JoyStick_Movement script
        joystick_movement = GetComponent<JoyStick_Movement>();

        // Subscribe to the action in JoyStick.cs
        joystick.onJoyStickMoved += GetJoyStickDirection;
    }

    void GetJoyStickDirection(Vector2 touchPosition)
    {
        // Touch direction updating every time joystick is moved.
        Vector2 touchDirection = new Vector2(touchPosition.x, touchPosition.y); // Assuming you want to move on the XZ plane
        // Call the player move function from JoyStick_Movement and pass the touchDirection
        joystick_movement.move(touchDirection);
    }
}


