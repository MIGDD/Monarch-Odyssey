using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerMovement : MonoBehaviour
{
    public static StackerMovement instance;

    private float mousePos;

    public int stacked = 0;
    
    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        // Clamp to actual screen width so the basket can reach the right edge
        // on displays wider than 2000px (the previous hardcoded max).
        mousePos = Mathf.Clamp(Input.mousePosition.x, 35.5f, Screen.width);

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos, 100f, 10f));

        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Acorn 1(Clone)" || collision.gameObject.name == "Apple 1(Clone)")
        {
            // Disable collider immediately so the fruit can't also enter the
            // floor's miss-trigger before Destroy completes at end of frame.
            collision.enabled = false;
            Destroy(collision.gameObject);
            stacked++;
        }
    }
}
