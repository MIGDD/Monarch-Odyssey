using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleLine : MonoBehaviour
{
    bool visible = false;
    // Start is called before the first frame update
    bool off = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            visible = true;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            visible = true;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            visible = true;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            visible = true;
        }

        else
        {
            visible = false;
        }

        if (visible == true)
        { // change name in find to the name of the aim indicator
            off = false;

            gameObject.transform.Find("Cube").gameObject.SetActive(true);
            

        }

        if (visible == false && off == false)
        {
            turnoff();
        }

    }
    private void turnoff()
    {
        gameObject.transform.Find("Cube").gameObject.SetActive(false);
        off = true;
    }
}
