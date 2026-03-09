using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAsteroid_Laser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            Destroy(other.gameObject);
        }
        }

    }
