using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingScript : MonoBehaviour
{
    
    void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
