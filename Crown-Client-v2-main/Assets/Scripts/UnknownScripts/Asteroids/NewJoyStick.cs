using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class NewJoystick_Movement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Rigidbody _rigidbody;
    //this might cause a problem below 
    [SerializeField] private joystick _joystick;

    //serializefield animator?

    [SerializeField] private float _moveSpeed;


    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

    }
}
