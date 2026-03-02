using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMovement : MonoBehaviour
{
    public int killerSpeed = -1;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        rb.transform.Translate(0,0,killerSpeed*Time.deltaTime);
    }
}
