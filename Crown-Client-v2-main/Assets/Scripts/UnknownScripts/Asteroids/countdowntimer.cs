using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdowntimer : MonoBehaviour
{
    public static float currentTime = 1f; // if it breaks add static before float. check spawnobjects because that will break if you do
    public  float startingTime = 30f;
    public Text countdowntext;
    
    //text
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdowntext.text = currentTime.ToString("0");

        if(currentTime < 0)
        {
            currentTime = 0;
        }
    }
}
