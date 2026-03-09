using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_follow : MonoBehaviour
{

    public Transform player;

    //These variables are just for editing until camera is in the right spot
    public float x;
    public float y;
    public float z;


    
    void LateUpdate() //keeps the camera at a certain distance from the player
    {
        transform.position = player.transform.position + new Vector3(x, y, z);
    }
}
