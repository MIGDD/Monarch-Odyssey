using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedButton : FishButton
{
    public GameObject ring;
    float xScale = 2.5f;
    float yScale = 2.5f;

    private void Update() {
        if (successfullyClicked || unsuccessfullyClicked) {
            TimedDestroy();
        } else {
            Shrink();
        }
    }
    /// <summary>
    /// Decreases the size of the ring by a speed constant
    /// </summary>
    void Shrink() {
        xScale -= Time.deltaTime / (speedConstant - difficulty) * 1.5f;
        yScale -= Time.deltaTime / (speedConstant - difficulty) * 1.5f;
        ring.GetComponent<Transform>().localScale = new Vector2(xScale, yScale);
        //Once ring becomes too small, automatically count it as a miss
        if (xScale <= 0.3f) {
            unsuccessfullyClicked = true;
            ring.GetComponent<Image>().color = Color.red;
            fishManager.GetComponent<FishManager>().ButtonMissed();
        }
    }
    /// <summary>
    /// Confirms if button has been clicked within the proper range
    /// </summary>
    public void Clicked() {
        if (!successfullyClicked && !unsuccessfullyClicked) {
            //Only count the button if it is clicked within specific range
            if (xScale <= 1.1f && xScale >= 0.5f) {
                successfullyClicked = true;
                ring.GetComponent<Image>().color = Color.green;
                fishManager.GetComponent<FishManager>().ButtonClicked();
            } else {
                unsuccessfullyClicked = true;
                ring.GetComponent<Image>().color = Color.red;
                fishManager.GetComponent<FishManager>().ButtonMissed();
            }
        }
    }
}
