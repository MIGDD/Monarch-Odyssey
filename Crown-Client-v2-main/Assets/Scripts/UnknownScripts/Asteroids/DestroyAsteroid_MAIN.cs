using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid_MAIN : MonoBehaviour
{
    bool canfire = true;
    

    [SerializeField] float laserOffTime = 0.5f;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && canfire == true){
            Invoke("FireLaser", 0.0f);
            

        }


    }

    public void FireLaser()
    {
        gameObject.transform.Find("laser").gameObject.SetActive(true);
        
        canfire = false;
        Invoke("TurnOffLaser", laserOffTime);

    }

    void TurnOffLaser()
    {
        gameObject.transform.Find("laser").gameObject.SetActive(false);
        canfire = true;
    }


    }

