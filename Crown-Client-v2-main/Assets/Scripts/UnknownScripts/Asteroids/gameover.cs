using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameover : MonoBehaviour
{
   // public Text pointsText;
   public void Update()
        
    {

        gameObject.SetActive(true);
        if (countdowntimer.currentTime == 0)
        {
            gameObject.transform.Find("Background").gameObject.SetActive(true);
        }



        // pointsTextText.text = score.ToString() + " Points"; //for points in the future
    }
}
